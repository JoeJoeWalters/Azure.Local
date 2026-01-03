using Azure.Local.Application.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Infrastructure.Timesheets.FileProcessing;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;
using Azure.Local.Tests.Unit.Timesheets.Helpers;

namespace Azure.Local.Tests.Unit.Timesheets
{
    public class ApplicationUnitTests
    {
        private readonly TimesheetApplication _application;

        public ApplicationUnitTests()
        {
            FakeRepository<TimesheetRepositoryItem> _repository = new();
            _application = new TimesheetApplication(
                repository: _repository,
                fileProcessor: new TimesheetFileProcessor(_repository)
            );
        }

        [Fact]
        public void GetById_Exists_ShouldBeSuccess()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);
            _ = _application.AddAsync(personId, testItem);

            // Act
            var result = _application.GetAsync(personId, testItem.Id);

            // Assert   
            result.Should().NotBeNull();
            testItem.Id.Should().Be(result?.Result?.Id);
        }

        [Fact]
        public async Task GetById_NotExists_ShouldBeFailure()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();

            // Act
            var result = await _application.GetAsync(personId, Guid.NewGuid().ToString());

            // Assert   
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddNewItem_NotExists_ShouldBeSuccess()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);

            // Act
            var result = await _application.AddAsync(personId, testItem);

            // Assert   
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AddNewItem_Exists_ShouldBeFailure()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);
            _ = _application.AddAsync(personId, testItem);

            // Act
            var result = await _application.AddAsync(personId, testItem);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);

            // Act
            var result = await _application.UpdateAsync(personId, testItem);

            // Assert   
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);
            _ = _application.AddAsync(personId, testItem);

            // Act
            var result = await _application.UpdateAsync(personId, testItem);

            // Assert   
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteItem_NotExists_ShouldBeFailure()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);

            // Act
            var result = await _application.DeleteAsync(personId, testItem.Id);

            // Assert   
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteItem_Exists_ShouldBeSuccess()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);
            _ = _application.AddAsync(personId, testItem);

            // Act
            var result = await _application.DeleteAsync(personId, testItem.Id);

            // Assert   
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SearchItems_Exists_ShouldReturnItems()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();
            var testItem = TestHelper.CreateTestItem(personId);
            _ = _application.AddAsync(personId, testItem);

            // Act
            var result = await _application.SearchAsync(personId, testItem.From.AddDays(-1), testItem.To.AddDays(1));

            // Assert   
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task SearchItems_NotExists_ShouldBeEmptyList()
        {
            // Arrange
            string personId = Guid.NewGuid().ToString();

            // Act
            var result = await _application.SearchAsync(personId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            // Assert   
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
    }
}
