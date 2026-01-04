using Azure.Local.Infrastructure.Timesheets.FileProcessing;
using Azure.Local.Infrastructure.Timesheets.FileProcessing.Converters;

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
    }
}
