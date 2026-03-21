using Azure.Local.Domain.Timesheets;
using System.Globalization;
using System.Net;
using System.Text;

namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class TimesheetHtmlDocumentBuilder : ITimesheetHtmlDocumentBuilder
    {
        public string Build(TimesheetItem item)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"  <title>Timesheet {Encode(item.Id)}</title>");
            sb.AppendLine("  <style>");
            sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 24px; color: #1f2937; }");
            sb.AppendLine("    h1, h2 { margin-bottom: 8px; }");
            sb.AppendLine("    table { width: 100%; border-collapse: collapse; margin-top: 12px; }");
            sb.AppendLine("    th, td { border: 1px solid #d1d5db; padding: 8px; text-align: left; }");
            sb.AppendLine("    th { background-color: #f3f4f6; }");
            sb.AppendLine("    .meta { margin-top: 4px; }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine($"  <h1>Timesheet {Encode(item.Id)}</h1>");
            sb.AppendLine($"  <div class=\"meta\"><strong>Person ID:</strong> {Encode(item.PersonId)}</div>");
            sb.AppendLine($"  <div class=\"meta\"><strong>Period:</strong> {FormatDate(item.From)} to {FormatDate(item.To)}</div>");
            sb.AppendLine($"  <div class=\"meta\"><strong>Status:</strong> {Encode(item.Status.ToString())}</div>");
            sb.AppendLine($"  <div class=\"meta\"><strong>Total Units:</strong> {item.TotalUnits.ToString("0.##", CultureInfo.InvariantCulture)}</div>");
            sb.AppendLine($"  <div class=\"meta\"><strong>Created By:</strong> {Encode(item.CreatedBy)}</div>");
            sb.AppendLine("  <h2>Components</h2>");
            sb.AppendLine("  <table>");
            sb.AppendLine("    <thead>");
            sb.AppendLine("      <tr>");
            sb.AppendLine("        <th>Id</th><th>From</th><th>To</th><th>Units</th><th>Project</th><th>Time Code</th><th>Work Type</th><th>Billable</th><th>Locked</th>");
            sb.AppendLine("      </tr>");
            sb.AppendLine("    </thead>");
            sb.AppendLine("    <tbody>");

            foreach (var component in item.Components)
            {
                sb.AppendLine("      <tr>");
                sb.AppendLine($"        <td>{Encode(component.Id)}</td>");
                sb.AppendLine($"        <td>{FormatDate(component.From)}</td>");
                sb.AppendLine($"        <td>{FormatDate(component.To)}</td>");
                sb.AppendLine($"        <td>{component.Units.ToString("0.##", CultureInfo.InvariantCulture)}</td>");
                sb.AppendLine($"        <td>{Encode(component.ProjectCode)}</td>");
                sb.AppendLine($"        <td>{Encode(component.TimeCode)}</td>");
                sb.AppendLine($"        <td>{Encode(component.WorkType.ToString())}</td>");
                sb.AppendLine($"        <td>{(component.IsBillable ? "Yes" : "No")}</td>");
                sb.AppendLine($"        <td>{(component.IsLocked ? "Yes" : "No")}</td>");
                sb.AppendLine("      </tr>");
            }

            sb.AppendLine("    </tbody>");
            sb.AppendLine("  </table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private static string FormatDate(DateTime value)
            => value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss 'UTC'", CultureInfo.InvariantCulture);

        private static string Encode(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
