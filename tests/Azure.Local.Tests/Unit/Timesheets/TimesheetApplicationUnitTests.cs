using AwesomeAssertions;
using Azure.Local.ApiService.Tests.Component.Fakes.Repositories;
using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Timesheets;
using static System.Net.Mime.MediaTypeNames;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    public class TimesheetApplicationUnitTests : TimeseetUnitTests
    {
        private readonly ITimesheetApplication _application;

        public TimesheetApplicationUnitTests()
        {
            _application = new TimesheetApplication(
                repository: new FakeRepository<TimesheetRepositoryItem>()
            );
        }

        [Fact]
        public async Task GetById_WithExistingItem_ShouldBeSuccess()
        {
            // Arrange
            var testItem = base.CreateTestItem();
            _application.AddAsync(testItem).Wait();

            // Act
            var result = _application.GetAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            testItem.Id.Should().Be(result?.Result?.Id);
        }

        [Fact]
        public async Task GetById_WithUnknownItem_ShouldBeFailure()
        {
            // Arrange

            // Act
            var result = _application.GetAsync(Guid.NewGuid().ToString());

            // Assert   
            result.Result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItem_NotExists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = base.CreateTestItem();

            // Act
            var result = _application.AddAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task AddNewItem_Exists_ShouldBeFailure()
        {
            // Arrange
            var testItem = base.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.AddAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = base.CreateTestItem();

            // Act
            var result = _application.UpdateAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = base.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.UpdateAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = base.CreateTestItem();

            // Act
            var result = _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = base.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }
    }
}
