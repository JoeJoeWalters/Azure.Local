using Azure.Local.ApiService.Versioning;
using Microsoft.AspNetCore.Http;

namespace Azure.Local.Tests.Unit.Versioning
{
    [ExcludeFromCodeCoverage]
    public class ApiVersionStrategyUnitTests
    {
        private readonly ApiVersionStrategy _sut = new();

        [Fact]
        public void GetRequestedVersion_ShouldReturnDefault_WhenHeaderMissing()
        {
            var context = new DefaultHttpContext();

            var result = _sut.GetRequestedVersion(context);

            result.Should().Be(ApiVersioningConstants.V1);
        }

        [Fact]
        public void GetRequestedVersion_ShouldReturnDefault_WhenHeaderWhitespace()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[ApiVersioningConstants.HeaderName] = "   ";

            var result = _sut.GetRequestedVersion(context);

            result.Should().Be(ApiVersioningConstants.V1);
        }

        [Fact]
        public void GetRequestedVersion_ShouldReturnHeaderVersion_WhenPresent()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[ApiVersioningConstants.HeaderName] = "1.0";

            var result = _sut.GetRequestedVersion(context);

            result.Should().Be("1.0");
        }

        [Fact]
        public void GetRequestedVersion_ShouldReturnFirstVersion_WhenMultipleValuesProvided()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[ApiVersioningConstants.HeaderName] = "1.0,2.0";

            var result = _sut.GetRequestedVersion(context);

            result.Should().Be("1.0");
        }
    }
}
