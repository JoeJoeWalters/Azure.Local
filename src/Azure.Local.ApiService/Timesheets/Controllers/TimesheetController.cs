using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Test.Helpers;
using Azure.Local.Domain.Test;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test;
using Azure.Local.Infrastructure.Test.Specifications;
using Azure.Local.Infrastructure.Timesheets;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace Azure.Local.ApiService.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly IRepository<TimesheetRepositoryItem> _repository;
        private readonly IValidator<AddTimesheetHttpRequest> _addTestItemHttpRequestValidator;

        private const string _testId = "test-item-1";

        public TimesheetController(
            IRepository<TimesheetRepositoryItem> repository,
            IValidator<AddTimesheetHttpRequest> addTestItemHttpRequestValidator)
        {
            _repository = repository;
            _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
        }

        [HttpPost]
        public IActionResult Post(AddTimesheetHttpRequest request)
        {
            var validationResult = _addTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            TimesheetItem item = request.ToTimesheetItem();

            _repository.Add(new TimesheetRepositoryItem
            {
                Id = item.Id,
                Name = item.Name
            });

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repository.Query(new GetByIdSpecification(_testId), 1);

            if (result.Result.Any())
            {
                var item = new TimesheetItem
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