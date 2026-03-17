using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class RepositorySpecificationUnitTests
    {
        [Fact]
        public async Task GetByIdSpecification_ReturnsMatchingItem()
        {
            // Arrange
            var repository = new FakeRepository<TimesheetRepositoryItem>();
            var expected = CreateRepositoryItem("target-id", "person-1", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1));
            var other = CreateRepositoryItem("other-id", "person-1", DateTime.UtcNow.AddDays(-4), DateTime.UtcNow.AddDays(-3));
            _ = await repository.AddAsync(expected);
            _ = await repository.AddAsync(other);

            // Act
            var result = await repository.QueryAsync(new GetByIdSpecification(expected.Id), take: 1);

            // Assert
            result.Should().ContainSingle();
            result.Single().Id.Should().Be(expected.Id);
        }

        [Fact]
        public async Task GetByIdSpecification_ReturnsEmpty_WhenIdDoesNotExist()
        {
            // Arrange
            var repository = new FakeRepository<TimesheetRepositoryItem>();
            var existing = CreateRepositoryItem("existing-id", "person-1", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1));
            _ = await repository.AddAsync(existing);

            // Act
            var result = await repository.QueryAsync(new GetByIdSpecification("missing-id"), take: 1);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteByIdSpecification_DeletesMatchingItem()
        {
            // Arrange
            var repository = new FakeRepository<TimesheetRepositoryItem>();
            var toDelete = CreateRepositoryItem("delete-me", "person-1", DateTime.UtcNow.AddDays(-3), DateTime.UtcNow.AddDays(-2));
            var toKeep = CreateRepositoryItem("keep-me", "person-1", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            _ = await repository.AddAsync(toDelete);
            _ = await repository.AddAsync(toKeep);

            // Act
            var deleted = await repository.DeleteAsync(new DeleteByIdSpecification(toDelete.Id));
            var deletedItem = await repository.QueryAsync(new GetByIdSpecification(toDelete.Id), take: 1);
            var keptItem = await repository.QueryAsync(new GetByIdSpecification(toKeep.Id), take: 1);

            // Assert
            deleted.Should().BeTrue();
            deletedItem.Should().BeEmpty();
            keptItem.Should().ContainSingle();
        }

        [Fact]
        public async Task DeleteByIdSpecification_ReturnsFalse_WhenIdDoesNotExist()
        {
            // Arrange
            var repository = new FakeRepository<TimesheetRepositoryItem>();
            var existing = CreateRepositoryItem("existing-id", "person-1", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1));
            _ = await repository.AddAsync(existing);

            // Act
            var deleted = await repository.DeleteAsync(new DeleteByIdSpecification("missing-id"));

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public async Task TimesheetSearchSpecification_FiltersByPersonAndDateOverlap()
        {
            // Arrange
            var repository = new FakeRepository<TimesheetRepositoryItem>();
            var searchFrom = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc);
            var searchTo = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc);

            var matchingInside = CreateRepositoryItem("inside", "person-1", new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 13, 0, 0, 0, DateTimeKind.Utc));
            var matchingStartBoundary = CreateRepositoryItem("start-boundary", "person-1", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), searchFrom);
            var matchingEndBoundary = CreateRepositoryItem("end-boundary", "person-1", searchTo, new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc));
            var beforeRange = CreateRepositoryItem("before", "person-1", new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc));
            var wrongPerson = CreateRepositoryItem("wrong-person", "person-2", new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 13, 0, 0, 0, DateTimeKind.Utc));

            foreach (var item in new[] { matchingInside, matchingStartBoundary, matchingEndBoundary, beforeRange, wrongPerson })
            {
                _ = await repository.AddAsync(item);
            }

            // Act
            var result = await repository.QueryAsync(new TimesheetSearchSpecification("person-1", searchFrom, searchTo));
            var resultIds = result.Select(x => x.Id).ToList();

            // Assert
            resultIds.Should().HaveCount(3);
            resultIds.Should().Contain(new[] { "inside", "start-boundary", "end-boundary" });
            resultIds.Should().NotContain(new[] { "before", "wrong-person" });
        }

        private static TimesheetRepositoryItem CreateRepositoryItem(string id, string personId, DateTime from, DateTime to)
            => new()
            {
                Id = id,
                PersonId = personId,
                From = from,
                To = to,
                CreatedBy = personId
            };
    }
}
