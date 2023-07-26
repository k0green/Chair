using Chair.BLL.CQRS.ExecutorProfile;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("executor-profile")]
    public class ExecutorProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutorProfileController> _logger;

        public ExecutorProfileController(IMediator mediator,
            ILogger<ExecutorProfileController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{serviceTypeId}")]
        [ProducesResponseType(typeof(List<ExecutorProfileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid serviceTypeId)
        {
            var query = new GetAllProfilesByServiceTypeIdQuery() { ServiceTypeId = serviceTypeId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-by-id/{id:guid}")]
        [ProducesResponseType(typeof(ExecutorProfileDto), 200)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetExecutorProfileByIdQuery() { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] AddExecutorProfileDto addExecutorProfileDto)
        {
            var command = new AddExecutorProfileQuery() { AddExecutorProfileDto = addExecutorProfileDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateExecutorProfileDto updateExecutorProfileDto)
        {
            var command = new UpdateExecutorProfileQuery() { UpdateExecutorProfileDto = updateExecutorProfileDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveExecutorProfileQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}