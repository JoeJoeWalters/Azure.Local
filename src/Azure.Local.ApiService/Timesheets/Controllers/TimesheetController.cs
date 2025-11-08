using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Test.Helpers;
using Azure.Local.Domain.Timesheets;
using Azure.Local.Infrastructure.Repository;
using Azure.Local.Infrastructure.Test.Specifications;
using Azure.Local.Infrastructure.Timesheets;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Local.ApiService.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly IRepository<TimesheetRepositoryItem> _repository;
        private readonly IValidator<AddTimesheetHttpRequest> _addTestItemHttpRequestValidator;

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
                From = item.From,
                To = item.To,
                Components = item.Components.Select(c => new TimesheetComponentRepositoryItem
                {
                    Units = c.Units,
                    From = c.From,
                    To = c.To
                }).ToList()
            });

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _repository.Query(new GetByIdSpecification(id), 1);

            if (result.Result.Any())
            {
                var first = result.Result.First();

                var item = new TimesheetItem
                {
                    Id = first.Id,
                    From = first.From,
                    To = first.To,
                    Components = first.Components.Select(c => new TimesheetComponentItem
                    {
                        Units = c.Units,
                        From = c.From,
                        To = c.To
                    }).ToList()
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