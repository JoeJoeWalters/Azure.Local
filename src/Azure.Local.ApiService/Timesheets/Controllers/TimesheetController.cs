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
        private readonly IValidator<PatchTimesheetHttpRequest> _patchTestItemHttpRequestValidator;

        public TimesheetController(
            ITimesheetApplication timesheetApplication,
            IValidator<AddTimesheetHttpRequest> addTestItemHttpRequestValidator,
            IValidator<PatchTimesheetHttpRequest> patchTestItemHttpRequestValidator)
        {
            _timesheetApplication = timesheetApplication;
            _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
            _patchTestItemHttpRequestValidator = patchTestItemHttpRequestValidator;
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
                var saveResult = await _timesheetApplication.AddAsync(item);
                if (saveResult)
                    return Ok();
                else
                    return Conflict();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(PatchTimesheetHttpRequest request)
        {
            var validationResult = _patchTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            TimesheetItem item = request.ToTimesheetItem();

            try
            {
                var saveResult = await _timesheetApplication.UpdateAsync(item);
                if (saveResult)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _timesheetApplication.GetAsync(id);
                if (result != null)
                    return new OkObjectResult(result);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _timesheetApplication.DeleteAsync(id);
                if (result)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}