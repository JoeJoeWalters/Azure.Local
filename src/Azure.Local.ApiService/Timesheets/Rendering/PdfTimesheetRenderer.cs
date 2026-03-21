using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class PdfTimesheetRenderer(
        ITimesheetHtmlDocumentBuilder htmlDocumentBuilder,
        IHtmlToPdfConverter htmlToPdfConverter) : ITimesheetRenderer
    {
        public TimesheetRenderOutputType OutputType => TimesheetRenderOutputType.Pdf;

        public async Task<TimesheetRenderResult> RenderAsync(TimesheetItem item, CancellationToken cancellationToken = default)
        {
            var htmlDocument = htmlDocumentBuilder.Build(item);
            var pdfBytes = await htmlToPdfConverter.ConvertAsync(htmlDocument, cancellationToken);
            return new TimesheetRenderResult
            {
                ContentType = "application/pdf",
                Content = pdfBytes,
                FileDownloadName = $"{item.Id}.pdf"
            };
        }
    }
}
