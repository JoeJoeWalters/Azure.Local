namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public interface IHtmlToPdfConverter
    {
        Task<byte[]> ConvertAsync(string html, CancellationToken cancellationToken = default);
    }
}
