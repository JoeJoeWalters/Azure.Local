using Azure.Local.Domain.Timesheets;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters
{
    public class StandardCsvFileConverter : IFileConverter
    {
        public async Task<TimesheetItem?> ConvertAsync(string personId, Stream fileStream)
        {
            if (fileStream == null || !fileStream.CanRead)
                return null;

            using var reader = new StreamReader(fileStream);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().DateTimeStyle = DateTimeStyles.AssumeUniversal;
            //csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = ["yyyy-MM-dd HH:mm:nn"];

            var records = new List<CsvRecord>();
            await foreach (var record in csv.GetRecordsAsync<CsvRecord>())
            {
                if (record.PersonId == personId)
                {
                    records.Add(record);
                }
            }

            if (records.Count == 0)
                return null;

            var timesheetItem = new TimesheetItem
            {
                PersonId = personId,
                From = records.Min(r => r.From),
                To = records.Max(r => r.To),
                CreatedBy = personId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = TimesheetStatus.Draft
            };

            foreach (var record in records)
            {
                timesheetItem.Components.Add(new TimesheetComponentItem
                {
                    From = record.From,
                    To = record.To,
                    Units = record.Units,
                    TimeCode = record.TimeCode,
                    ProjectCode = record.ProjectCode
                });
            }

            return timesheetItem;
        }

        private class CsvRecord
        {
            public string PersonId { get; set; } = string.Empty;
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public double Units { get; set; }
            public string TimeCode { get; set; } = string.Empty;
            public string ProjectCode { get; set; } = string.Empty;
        }
    }
}
