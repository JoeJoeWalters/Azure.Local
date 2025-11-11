using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Test.Helpers;
using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Local.ApiService.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetApplication _timesheetApplication;
        private readonly IValidator<AddTimesheetHttpRequest> _addTestItemHttpRequestValidator;

        public TimesheetController(
            ITimesheetApplication timesheetApplication,
            IValidator<AddTimesheetHttpRequest> addTestItemHttpRequestValidator)
        {
            _timesheetApplication = timesheetApplication;
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

            if (_timesheetApplication.Save(item))
                return Ok();
            else
                return StatusCode(500, "Failed to save timesheet item.");
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = _timesheetApplication.GetById(id);
            if (result != null)
                return new OkObjectResult(result);
            else
                return NotFound();
        }
    }
}