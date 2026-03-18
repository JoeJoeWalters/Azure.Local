using Azure.Local.ApiService.Timesheets.Contracts.V1;
using Azure.Local.ApiService.Timesheets.Mapping.V1;
using Azure.Local.ApiService.Versioning;
using Azure.Local.Application.Timesheets;
using Azure.Local.Application.Timesheets.V1;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersioningConstants.V1)]
    [Route("person")]
    public class TimesheetControllerV1(
        ITimesheetApplicationV1 timesheetApplication,
        ITimesheetContractMapperV1 mapper,
        IValidator<AddTimesheetHttpRequestV1> addTestItemHttpRequestValidator,
        IValidator<PatchTimesheetHttpRequestV1> patchTestItemHttpRequestValidator,
        IValidator<ChangeTimesheetStateHttpRequestV1> changeTimesheetStateHttpRequestValidator) : ControllerBase
    {
        private readonly ITimesheetApplicationV1 _timesheetApplication = timesheetApplication;
        private readonly ITimesheetContractMapperV1 _mapper = mapper;
        private readonly IValidator<AddTimesheetHttpRequestV1> _addTestItemHttpRequestValidator = addTestItemHttpRequestValidator;
        private readonly IValidator<PatchTimesheetHttpRequestV1> _patchTestItemHttpRequestValidator = patchTestItemHttpRequestValidator;
        private readonly IValidator<ChangeTimesheetStateHttpRequestV1> _changeTimesheetStateHttpRequestValidator = changeTimesheetStateHttpRequestValidator;

        [HttpPost("{personId}/timesheet/item")]
        public async Task<IActionResult> Post([FromRoute] string personId, AddTimesheetHttpRequestV1 request)
        {
            if (request.PersonId != personId)
                return BadRequest("PersonId in URL does not match PersonId in request body.");

            // Set CreatedBy from personId if not provided (in real app, use authenticated user)
            if (string.IsNullOrEmpty(request.CreatedBy))
                request.CreatedBy = personId;

            var validationResult = _addTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var item = _mapper.ToDomain(request);

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
        public async Task<IActionResult> Patch([FromRoute] string personId, PatchTimesheetHttpRequestV1 request)
        {
            if (request.PersonId != personId)
                return BadRequest("PersonId in URL does not match PersonId in request body.");

            var validationResult = _patchTestItemHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var item = _mapper.ToDomain(request);
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
                    ? new OkObjectResult(_mapper.ToResponse(result)) 
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
                var responses = _mapper.ToResponse(results);
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

        [HttpPost("{personId}/timesheet/item/{id}/state")]
        public async Task<IActionResult> ChangeState([FromRoute] string personId, [FromRoute] string id, ChangeTimesheetStateHttpRequestV1 request)
        {
            var validationResult = _changeTimesheetStateHttpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var message = await _timesheetApplication.ChangeStateAsync(personId, id, personId, request.State, request.Comments);
                return message is not null ? Ok(new { Message = message }) : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
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
                return BadRequest(new { ex.Message });
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
