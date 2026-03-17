using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Repository.Specifications;
using Azure.Local.Infrastructure.Repository.Specifications.Timesheets;
using Azure.Local.Infrastructure.Timesheets;
using Azure.Local.Tests.Component.Timesheets.Fakes.Repositories;

namespace Azure.Local.Tests.Unit.Timesheets.Repository
{
    [ExcludeFromCodeCoverage]
    public class TimesheetCosmosRepositoryUnitTests
    {
        [Fact]
        public async Task AddAsync_PersistsItem_AndReturnsTrue()
        {
            // Arrange
            var backingRepository = new FakeRepository<TimesheetRepositoryItem>();
            var sut = new TimesheetCosmosRepository(backingRepository);
            var item = CreateDomainItem("add-id", "person-1", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1));

            // Act
            var added = await sut.AddAsync(item);
            var stored = await backingRepository.QueryAsync(new GetByIdSpecification(item.Id), take: 1);

            // Assert
            added.Should().BeTrue();
            stored.Should().ContainSingle();
            stored.Single().PersonId.Should().Be(item.PersonId);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingItem_AndReturnsTrue()
        {
            // Arrange
            var backingRepository = new FakeRepository<TimesheetRepositoryItem>();
            var sut = new TimesheetCosmosRepository(backingRepository);
            var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc);
            var original = CreateDomainItem("update-id", "person-1", from, to);
            _ = await sut.AddAsync(original);

            original.Comments = "updated";
            original.ModifiedBy = "editor";

            // Act
            var updated = await sut.UpdateAsync(original);
            var stored = await backingRepository.QueryAsync(new GetByIdSpecification(original.Id), take: 1);

            // Assert
            updated.Should().BeTrue();
            stored.Should().ContainSingle();
            stored.Single().Comments.Should().Be("updated");
            stored.Single().ModifiedBy.Should().Be("editor");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedItem_WhenFound()
        {
            // Arrange
            var backingRepository = new FakeRepository<TimesheetRepositoryItem>();
            var sut = new TimesheetCosmosRepository(backingRepository);
            var repositoryItem = CreateRepositoryItem("existing-id", "person-1", DateTime.UtcNow.AddDays(-4), DateTime.UtcNow.AddDays(-3));
            _ = await backingRepository.AddAsync(repositoryItem);

            // Act
            var result = await sut.GetByIdAsync(repositoryItem.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(repositoryItem.Id);
            result.PersonId.Should().Be(repositoryItem.PersonId);
            result.Components.Should().ContainSingle();
            result.Components.Single().TimeCode.Should().Be(repositoryItem.Components.Single().TimeCode);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var sut = new TimesheetCosmosRepository(new FakeRepository<TimesheetRepositoryItem>());

            // Act
            var result = await sut.GetByIdAsync("missing-id");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteByIdAsync_RemovesMatchingItem()
        {
            // Arrange
            var backingRepository = new FakeRepository<TimesheetRepositoryItem>();
            var sut = new TimesheetCosmosRepository(backingRepository);
            var item = CreateRepositoryItem("delete-id", "person-1", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(-1));
            _ = await backingRepository.AddAsync(item);

            // Act
            var deleted = await sut.DeleteByIdAsync(item.Id);
            var postDelete = await backingRepository.QueryAsync(new GetByIdSpecification(item.Id), take: 1);

            // Assert
            deleted.Should().BeTrue();
            postDelete.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchAsync_FiltersByPersonAndDateOverlap_AndMapsResults()
        {
            // Arrange
            var backingRepository = new FakeRepository<TimesheetRepositoryItem>();
            var sut = new TimesheetCosmosRepository(backingRepository);
            var searchFrom = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc);
            var searchTo = new DateTime(2026, 2, 20, 0, 0, 0, DateTimeKind.Utc);

            var matching = CreateRepositoryItem("matching", "person-1", new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, DateTimeKind.Utc));
            var wrongPerson = CreateRepositoryItem("wrong-person", "person-2", new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 16, 0, 0, 0, DateTimeKind.Utc));
            var outOfRange = CreateRepositoryItem("out-of-range", "person-1", new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc));

            foreach (var item in new[] { matching, wrongPerson, outOfRange })
            {
                _ = await backingRepository.AddAsync(item);
            }

            // Act
            var result = (await sut.SearchAsync("person-1", searchFrom, searchTo)).ToList();

            // Assert
            result.Should().ContainSingle();
            result.Single().Id.Should().Be("matching");
            result.Single().Components.Should().ContainSingle();
        }

        private static TimesheetItem CreateDomainItem(string id, string personId, DateTime from, DateTime to)
            => new()
            {
                Id = id,
                PersonId = personId,
                From = from,
                To = to,
                CreatedBy = personId,
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = $"{id}-component",
                        Units = 8,
                        From = from,
                        To = from.AddHours(8),
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        Description = "Worked on task",
                        IsBillable = true
                    }
                ]
            };

        private static TimesheetRepositoryItem CreateRepositoryItem(string id, string personId, DateTime from, DateTime to)
            => new()
            {
                Id = id,
                PersonId = personId,
                From = from,
                To = to,
                CreatedBy = personId,
                Components =
                [
                    new TimesheetComponentRepositoryItem
                    {
                        Id = $"{id}-component",
                        Units = 8,
                        From = from,
                        To = from.AddHours(8),
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        Description = "Worked on task",
                        IsBillable = true
                    }
                ]
            };
    }
}
