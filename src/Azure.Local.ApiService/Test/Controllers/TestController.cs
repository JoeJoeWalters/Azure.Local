using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Test.Helpers;
using Azure.Local.Domain.Test;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test;
using Azure.Local.Infrastructure.Test.Specifications;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace Azure.Local.ApiService.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRepository<RepositoryTestItem> _repository;
        private readonly IValidator<AddTestItemHttpRequest> _addTestItemHttpRequestValidator;

        private const string _testId = "test-item-1";

        public TestController(
            IRepository<RepositoryTestItem> repository,
            IValidator<AddTestItemHttpRequest> addTestItemHttpRequestValidator)
        {
            _repository = repository;
            _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
        }

        [HttpPost]
        public IActionResult Post(AddTestItemHttpRequest request)
        {
            var validationResult = _addTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            TestItem item = request.ToTestItem();

            _repository.Add(new RepositoryTestItem
            {
                Id = item.Id,
                Name = item.Name
            });

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Query(new TestItemGetSpecification(_testId), 1);

            if (result.Result.Any())
            {
                var item = new TestItem
                {
                    Id = result.Result.First().Id,
                    Name = result.Result.First().Name
                };

                return new OkObjectResult(item);
            }
            else
            {
                return NotFound();
            }
        }
    }
}