using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ZXing;

namespace Azure.Local.Tests.Unit.Timesheets.Helpers
{
    [ExcludeFromCodeCoverage]
    internal static class TimesheetQrCodeTestHelper
    {
        public static Result Decode(string html)
        {
            var source = Regex.Match(html, "src=\"data:image/svg\\+xml;base64,([^\"]+)\"");
            source.Success.Should().BeTrue();
            var svg = XElement.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(source.Groups[1].Value)));
            var moduleCount = int.Parse(svg.Attribute("width")!.Value, CultureInfo.InvariantCulture);
            var modules = new bool[moduleCount, moduleCount];
            XNamespace ns = "http://www.w3.org/2000/svg";

            svg.Element(ns + "rect")!.Attribute("fill")!.Value.Should().Be("#ffffff");
            foreach (var path in svg.Elements(ns + "path"))
            {
                path.Attribute("fill")!.Value.Should().Be("#000000");
                // QRCoder represents each horizontal run of dark modules as an SVG rectangle path.
                foreach (Match run in Regex.Matches(path.Attribute("d")!.Value, @"M(\d+) (\d+)h(\d+)v1h-\d+z"))
                {
                    var x = int.Parse(run.Groups[1].Value, CultureInfo.InvariantCulture);
                    var y = int.Parse(run.Groups[2].Value, CultureInfo.InvariantCulture);
                    var width = int.Parse(run.Groups[3].Value, CultureInfo.InvariantCulture);
                    for (var offset = 0; offset < width; offset++)
                    {
                        modules[y, x + offset] = true;
                    }
                }
            }

            const int pixelsPerModule = 4;
            var size = moduleCount * pixelsPerModule;
            var pixels = new byte[size * size];
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var moduleX = x / pixelsPerModule;
                    var moduleY = y / pixelsPerModule;
                    var isDark = modules[moduleY, moduleX];
                    if (moduleX < 4 || moduleY < 4 || moduleX >= moduleCount - 4 || moduleY >= moduleCount - 4)
                    {
                        isDark.Should().BeFalse("the QR code must retain its four-module quiet zone");
                    }
                    pixels[y * size + x] = isDark ? (byte)0 : (byte)255;
                }
            }

            var result = new BarcodeReaderGeneric().Decode(pixels, size, size, RGBLuminanceSource.BitmapFormat.Gray8);
            result.Should().NotBeNull("the embedded SVG must contain a scannable QR code");
            result.BarcodeFormat.Should().Be(BarcodeFormat.QR_CODE);
            result.ResultMetadata[ResultMetadataType.ERROR_CORRECTION_LEVEL].Should().Be("H");
            return result;
        }
    }
}
