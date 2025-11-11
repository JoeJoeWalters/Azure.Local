using Azure.Local.ApiService.Test.Contracts;
using Azure.Local.ApiService.Test.Helpers;
using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> Post(AddTimesheetHttpRequest request)
        {
            var validationResult = _addTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            TimesheetItem item = request.ToTimesheetItem();

            try
            {
                var saveResult = await _timesheetApplication.SaveAsync(item);
                if (saveResult)
                    return Ok();
                else
                    return Conflict();
            }
            catch(Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _timesheetApplication.GetAsync(id);
            if (result != null)
                return new OkObjectResult(result);
            else
                return NotFound();
        }
    }
}