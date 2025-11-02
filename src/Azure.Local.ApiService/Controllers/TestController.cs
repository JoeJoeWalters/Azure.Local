using Azure.Local.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Local.ApiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRepository<RepositoryTestItem> _repository;

        private const string _testId = "test-item-1";

        public TestController(IRepository<RepositoryTestItem> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post()
        {
            _repository.Add(new RepositoryTestItem
            {
                Id = _testId,
                Name = "Test Item"
            });

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Query(new TestItemGetSpecification(_testId), 1);

            return Ok();
        }
    }
}