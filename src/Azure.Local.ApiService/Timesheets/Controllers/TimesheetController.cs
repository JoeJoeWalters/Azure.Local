using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.ApiService.Timesheets.Helpers;
using Azure.Local.Application.Timesheets;
using Azure.Local.Domain.Timesheets;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers
{
    [ApiController]
    [Route("person")]
    public class TimesheetController(
        ITimesheetApplication timesheetApplication,
        IValidator<AddTimesheetHttpRequest> addTestItemHttpRequestValidator,
        IValidator<PatchTimesheetHttpRequest> patchTestItemHttpRequestValidator) : ControllerBase
    {
        private readonly ITimesheetApplication _timesheetApplication = timesheetApplication;
        private readonly IValidator<AddTimesheetHttpRequest> _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
        private readonly IValidator<PatchTimesheetHttpRequest> _patchTestItemHttpRequestValidator = patchTestItemHttpRequestValidator;

        [HttpPost("{personId}/timesheet/item")]
        public async Task<IActionResult> Post([FromRoute] string personId, AddTimesheetHttpRequest request)
        {
            if (request.PersonId != personId)
                return BadRequest("PersonId in URL does not match PersonId in request body.");

            var validationResult = _addTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            TimesheetItem item = request.ToTimesheetItem();

            try
            {
                var saveResult = await _timesheetApplication.AddAsync(personId, item);
                return saveResult ? Ok() : Conflict();
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPatch("{personId}/timesheet/item")]
        public async Task<IActionResult> Patch([FromRoute] string personId, PatchTimesheetHttpRequest request)
        {
            if (request.PersonId != personId)
                return BadRequest("PersonId in URL does not match PersonId in request body.");

            var validationResult = _patchTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            TimesheetItem item = request.ToTimesheetItem();

            try
            {
                var saveResult = await _timesheetApplication.UpdateAsync(personId, item);
                return saveResult ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet("{personId}/timesheet/item/{id}")]
        public async Task<IActionResult> Get([FromRoute] string personId, [FromRoute] string id)
        {
            try
            {
                var result = await _timesheetApplication.GetAsync(personId, id);
                return (result != null) ? new OkObjectResult(result) : NotFound();
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet("{personId}/timesheet/search")]
        public async Task<IActionResult> Search([FromRoute] string personId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            try
            {
                // Always a result (unless exception) but could be empty list
                return new OkObjectResult(await _timesheetApplication.SearchAsync(personId, fromDate, toDate));
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpDelete("{personId}/timesheet/item/{id}")]
        public async Task<IActionResult> Delete(string personId, string id)
        {
            try
            {
                var result = await _timesheetApplication.DeleteAsync(personId, id);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}