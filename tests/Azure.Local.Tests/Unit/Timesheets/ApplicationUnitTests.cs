using AwesomeAssertions;
using Azure.Local.ApiService.Tests.Component.Timesheets.Fakes.Repositories;
using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Timesheets;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    public class ApplicationUnitTests
    {
        private readonly ITimesheetApplication _application;

        public ApplicationUnitTests()
        {
            _application = new TimesheetApplication(
                repository: new FakeRepository<TimesheetRepositoryItem>()
            );
        }

        [Fact]
        public void GetById_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            _ = _application.AddAsync(testItem);

            // Act
            var result = _application.GetAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            testItem.Id.Should().Be(result?.Result?.Id);
        }

        [Fact]
        public void GetById_NotExists_ShouldBeFailure()
        {
            // Arrange

            // Act
            var result = _application.GetAsync(Guid.NewGuid().ToString());

            // Assert   
            result.Result.Should().BeNull();
        }

        [Fact]
        public void AddNewItem_NotExists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = _application.AddAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void AddNewItem_Exists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.AddAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void UpdateItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = _application.UpdateAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void UpdateItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.UpdateAsync(testItem);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }

        [Fact]
        public void DeleteItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeFalse();
        }

        [Fact]
        public void DeleteItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            var resultFirst = _application.AddAsync(testItem);

            // Act
            var result = _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            result.Result.Should().BeTrue();
        }
    }
}
