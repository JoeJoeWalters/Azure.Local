using Azure.Local.ApiService.Timesheets.Rendering;
using System.Text;

namespace Azure.Local.Tests.Component.Timesheets.Fakes.Rendering
{
    [ExcludeFromCodeCoverage]
    public sealed class FakeHtmlToPdfConverter : IHtmlToPdfConverter
    {
        public Task<byte[]> ConvertAsync(string html, CancellationToken cancellationToken = default)
        {
            var payload = Encoding.UTF8.GetBytes($"%PDF-FAKE%{html}");
            return Task.FromResult(payload);
        }
    }
}
