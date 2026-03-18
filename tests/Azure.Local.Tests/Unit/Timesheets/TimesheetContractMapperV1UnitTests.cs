using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Contracts.V1;
using Azure.Local.ApiService.Timesheets.Mapping.V1;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetContractMapperV1UnitTests
    {
        private readonly TimesheetContractMapperV1 _sut = new();

        [Fact]
        public void ToDomain_FromAddRequest_ShouldMap()
        {
            var request = CreateAddRequest();

            var result = _sut.ToDomain(request);

            result.Id.Should().Be(request.Id);
            result.PersonId.Should().Be(request.PersonId);
            result.Components.Should().ContainSingle();
        }

        [Fact]
        public void ToDomain_FromPatchRequest_ShouldMap()
        {
            var request = new PatchTimesheetHttpRequestV1
            {
                Id = "ts-1",
                PersonId = "person-1",
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow,
                CreatedBy = "person-1",
                Components =
                [
                    new TimesheetHttpRequestComponentV1
                    {
                        Id = "c-1",
                        Units = 8,
                        From = DateTime.UtcNow.AddHours(-8),
                        To = DateTime.UtcNow,
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        WorkType = "Regular",
                        IsBillable = true
                    }
                ]
            };

            var result = _sut.ToDomain(request);

            result.Id.Should().Be(request.Id);
            result.PersonId.Should().Be(request.PersonId);
            result.Components.Should().ContainSingle();
        }

        [Fact]
        public void ToResponse_ShouldMap()
        {
            var item = CreateDomainItem("ts-1");

            var result = _sut.ToResponse(item);

            result.Id.Should().Be(item.Id);
            result.PersonId.Should().Be(item.PersonId);
            result.Components.Should().ContainSingle();
            result.TotalUnits.Should().Be(item.TotalUnits);
        }

        [Fact]
        public void ToResponseList_ShouldMapAll()
        {
            var items = new List<TimesheetItem>
            {
                CreateDomainItem("ts-1"),
                CreateDomainItem("ts-2")
            };

            var result = _sut.ToResponse(items);

            result.Should().HaveCount(2);
            result.Select(r => r.Id).Should().BeEquivalentTo(["ts-1", "ts-2"]);
        }

        private static AddTimesheetHttpRequestV1 CreateAddRequest()
            => new()
            {
                Id = "ts-1",
                PersonId = "person-1",
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow,
                CreatedBy = "person-1",
                Components =
                [
                    new TimesheetHttpRequestComponentV1
                    {
                        Id = "c-1",
                        Units = 8,
                        From = DateTime.UtcNow.AddHours(-8),
                        To = DateTime.UtcNow,
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        WorkType = "Regular",
                        IsBillable = true
                    }
                ]
            };

        private static TimesheetItem CreateDomainItem(string id)
            => new()
            {
                Id = id,
                PersonId = "person-1",
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow,
                CreatedBy = "person-1",
                Components =
                [
                    new TimesheetComponentItem
                    {
                        Id = $"{id}-c1",
                        Units = 8,
                        From = DateTime.UtcNow.AddHours(-8),
                        To = DateTime.UtcNow,
                        TimeCode = "DEV",
                        ProjectCode = "PRJ",
                        WorkType = WorkType.Regular,
                        IsBillable = true
                    }
                ]
            };
    }
}
