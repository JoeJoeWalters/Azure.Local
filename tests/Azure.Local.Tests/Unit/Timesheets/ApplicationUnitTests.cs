using Azure.Local.ApiService.Tests.Unit.Timesheets;
using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUnitTests
    {
        private readonly TimesheetApplication _application;

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
        public async Task GetById_NotExists_ShouldBeFailure()
        {
            // Arrange

            // Act
            var result = await _application.GetAsync(Guid.NewGuid().ToString());

            // Assert   
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItem_NotExists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = await _application.AddAsync(testItem);

            // Assert   
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AddNewItem_Exists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            _ = _application.AddAsync(testItem);

            // Act
            var result = await _application.AddAsync(testItem);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = await _application.UpdateAsync(testItem);

            // Assert   
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            _ = _application.AddAsync(testItem);

            // Act
            var result = await _application.UpdateAsync(testItem);

            // Assert   
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();

            // Act
            var result = await _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            var testItem = TestHelper.CreateTestItem();
            _ = _application.AddAsync(testItem);

            // Act
            var result = await _application.DeleteAsync(testItem.Id);

            // Assert   
            result.Should().BeTrue();
        }
    }
}
