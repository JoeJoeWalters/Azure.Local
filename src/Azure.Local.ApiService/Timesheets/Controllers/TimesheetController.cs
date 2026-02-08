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
        IValidator<PatchTimesheetHttpRequest> patchTestItemHttpRequestValidator,
        IValidator<SubmitTimesheetHttpRequest> submitTimesheetHttpRequestValidator,
        IValidator<ApproveTimesheetHttpRequest> approveTimesheetHttpRequestValidator,
        IValidator<RejectTimesheetHttpRequest> rejectTimesheetHttpRequestValidator,
        IValidator<RecallTimesheetHttpRequest> recallTimesheetHttpRequestValidator) : ControllerBase
    {
        private readonly ITimesheetApplication _timesheetApplication = timesheetApplication;
        private readonly IValidator<AddTimesheetHttpRequest> _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
        private readonly IValidator<PatchTimesheetHttpRequest> _patchTestItemHttpRequestValidator = patchTestItemHttpRequestValidator;
        private readonly IValidator<SubmitTimesheetHttpRequest> _submitTimesheetHttpRequestValidator = submitTimesheetHttpRequestValidator;
        private readonly IValidator<ApproveTimesheetHttpRequest> _approveTimesheetHttpRequestValidator = approveTimesheetHttpRequestValidator;
        private readonly IValidator<RejectTimesheetHttpRequest> _rejectTimesheetHttpRequestValidator = rejectTimesheetHttpRequestValidator;
        private readonly IValidator<RecallTimesheetHttpRequest> _recallTimesheetHttpRequestValidator = recallTimesheetHttpRequestValidator;

        [HttpPost("{personId}/timesheet/item")]
        public async Task<IActionResult> Post([FromRoute] string personId, AddTimesheetHttpRequest request)
        {
            if (request.PersonId != personId)
                return BadRequest("PersonId in URL does not match PersonId in request body.");

            // Set CreatedBy from personId if not provided (in real app, use authenticated user)
            if (string.IsNullOrEmpty(request.CreatedBy))
                request.CreatedBy = personId;

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
            item.ModifiedDate = DateTime.UtcNow;
            item.ModifiedBy = personId; // In real app, use authenticated user

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
                return result != null 
                    ? new OkObjectResult(result.ToTimesheetResponse()) 
                    : NotFound();
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
                var results = await _timesheetApplication.SearchAsync(personId, fromDate, toDate);
                var responses = results.Select(r => r.ToTimesheetResponse()).ToList();
                return new OkObjectResult(responses);
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

        [HttpPost("{personId}/timesheet/item/{id}/submit")]
        public async Task<IActionResult> Submit([FromRoute] string personId, [FromRoute] string id, SubmitTimesheetHttpRequest request)
        {
            var validationResult = _submitTimesheetHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var result = await _timesheetApplication.SubmitAsync(personId, id, personId);
                return result ? Ok(new { Message = "Timesheet submitted successfully." }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost("{personId}/timesheet/item/{id}/approve")]
        public async Task<IActionResult> Approve([FromRoute] string personId, [FromRoute] string id, ApproveTimesheetHttpRequest request)
        {
            var validationResult = _approveTimesheetHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                // In real app, approvedBy should come from authenticated user
                var result = await _timesheetApplication.ApproveAsync(personId, id, personId);
                return result ? Ok(new { Message = "Timesheet approved successfully." }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost("{personId}/timesheet/item/{id}/reject")]
        public async Task<IActionResult> Reject([FromRoute] string personId, [FromRoute] string id, RejectTimesheetHttpRequest request)
        {
            var validationResult = _rejectTimesheetHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                // In real app, rejectedBy should come from authenticated user
                var result = await _timesheetApplication.RejectAsync(personId, id, personId, request.RejectionReason);
                return result ? Ok(new { Message = "Timesheet rejected." }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost("{personId}/timesheet/item/{id}/recall")]
        public async Task<IActionResult> Recall([FromRoute] string personId, [FromRoute] string id, RecallTimesheetHttpRequest request)
        {
            var validationResult = _recallTimesheetHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var result = await _timesheetApplication.RecallAsync(personId, id, personId);
                return result ? Ok(new { Message = "Timesheet recalled successfully." }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPost("{personId}/timesheet/item/{id}/reopen")]
        public async Task<IActionResult> Reopen([FromRoute] string personId, [FromRoute] string id)
        {
            try
            {
                var result = await _timesheetApplication.ReopenAsync(personId, id, personId);
                return result ? Ok(new { Message = "Timesheet reopened to Draft status." }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
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