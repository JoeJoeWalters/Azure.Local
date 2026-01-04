using Azure.Local.Infrastructure.Timesheets.FileProcessing;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;
using System.Globalization;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class FileProcessingUnitTests
    {
        private readonly FileConverterFactory _factory;

        public FileProcessingUnitTests()
        {
            _factory = new FileConverterFactory();
        }

        [Theory]
        [InlineData(TimesheetFileTypes.StandardCSVTemplate, typeof(StandardCsvFileConverter))]
        public void CreateConverter_WithEnum_ShouldReturnCorrectConverter(TimesheetFileTypes fileType, Type expectedType)
        {
            // Arrange

            // Act
            var result = _factory.CreateConverter(fileType);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(expectedType);
        }

        [Fact]
        public void CreateConverter_None_ShouldThrowArgumentException()
        {
            // Arrange
            var fileType = TimesheetFileTypes.None;

            // Act
            var act = () => _factory.CreateConverter(fileType);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("File type cannot be None*")
                .WithParameterName("fileType");
        }

        [Fact]
        public void CreateConverter_UnsupportedType_ShouldThrowNotSupportedException()
        {
            // Arrange
            var fileType = (TimesheetFileTypes)999;

            // Act
            var act = () => _factory.CreateConverter(fileType);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("File type '999' is not supported");
        }

        [Fact]
        public async Task StandardCsvFileConverter_ValidCsvFile_ShouldParseCorrectly()
        {
            // Arrange
            var converter = new StandardCsvFileConverter();
            var personId = "c8ba72eb-bef7-494c-8d21-f5915f4f71a9";
            var dailyUnitCode = "c8ba72eb-bef7-494c-8d21-f5915f4f71a5";
            var overtimeUnitCode = "c8ba72eb-bef7-494c-8d21-f5915f4f71a4";
            var testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Unit", "Timesheets", "TestFiles", "FileProcessing", "StandardCSV.csv");

            // Act
            using var fileStream = File.OpenRead(testFilePath);
            var result = await converter.ConvertAsync(personId, fileStream);

            // Assert
            result.Should().NotBeNull();
            result!.PersonId.Should().Be(personId);
            result.Components.Should().HaveCount(4);

            result.From.Should().Be(DateTime.Parse("2025-12-29 09:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
            result.To.Should().Be(DateTime.Parse("2025-12-31 17:30:00Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));

            result.Components.Where(t => t.Code == dailyUnitCode && t.Units == 7.5).Should().HaveCountGreaterThan(1);
            result.Components.Where(t => t.Code == overtimeUnitCode && t.Units == 4).Should().HaveCount(1);
        }

        [Fact]
        public async Task StandardCsvFileConverter_NullStream_ShouldReturnNull()
        {
            // Arrange
            var converter = new StandardCsvFileConverter();
            var personId = "test-person-id";

            // Act
            var result = await converter.ConvertAsync(personId, null!);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task StandardCsvFileConverter_EmptyFile_ShouldReturnNull()
        {
            // Arrange
            var converter = new StandardCsvFileConverter();
            var personId = "test-person-id";
            using var stream = new MemoryStream();

            // Act
            var result = await converter.ConvertAsync(personId, stream);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task StandardCsvFileConverter_PersonIdNotInFile_ShouldReturnNull()
        {
            // Arrange
            var converter = new StandardCsvFileConverter();
            var personId = "non-existent-person-id";
            var testFilePath = Path.Combine("Unit", "Timesheets", "TestFiles", "FileProcessing", "StandardCSV.csv");
            
            using var fileStream = File.OpenRead(testFilePath);

            // Act
            var result = await converter.ConvertAsync(personId, fileStream);

            // Assert
            result.Should().BeNull();
        }
    }
}
