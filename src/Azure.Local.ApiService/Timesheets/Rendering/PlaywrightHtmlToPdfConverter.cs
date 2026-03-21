using Microsoft.Playwright;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class PlaywrightHtmlToPdfConverter : IHtmlToPdfConverter
    {
        public async Task<byte[]> ConvertAsync(string html, CancellationToken cancellationToken = default)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(html, new PageSetContentOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            cancellationToken.ThrowIfCancellationRequested();
            return await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                PrintBackground = true
            });
        }
    }
}
