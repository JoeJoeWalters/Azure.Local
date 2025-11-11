using AwesomeAssertions;
using Azure.Local.ApiService.Tests.Component.Fakes.Repositories;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test.Specifications;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Infrastructure.Timesheets.Specifications;
using System.Security.Cryptography.Xml;

namespace Azure.Local.ApiService.Tests.Unit.Timesheets
{
    public class TimesheetSearchUnitTests : TimeseetUnitTests
    {
        [Fact]
        public async Task QueryRepositoryById_WithExistingItem_ShouldBeSuccess()
        {
            // Arrange
            var testItem = base.CreateTestItem();
            IRepository<TimesheetRepositoryItem> repository = 
                new FakeRepository<TimesheetRepositoryItem>();

            var addResult = await repository.AddAsync(testItem);

            GetByIdSpecification getByIdSpecification = 
                                    new GetByIdSpecification(
                                        Id: testItem.Id.ToString()
                                    );

            // Act
            var result = await repository.QueryAsync(getByIdSpecification, 1);

            // Assert   
            var firstItem = result.First();
            testItem.Id.Should().Be(firstItem.Id);
        }
    }
}
