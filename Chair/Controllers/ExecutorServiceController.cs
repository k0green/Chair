using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("executor-service")]
    public class ExecutorServiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutorServiceController> _logger;

        public ExecutorServiceController(IMediator mediator,
            ILogger<ExecutorServiceController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{executorId}")]
        [ProducesResponseType(typeof(List<ExecutorServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid executorId)
        {
            var query = new GetAllServicesByExecutorIdQuery() { ExecutorId = executorId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-by-id/{id:guid}")]
        [ProducesResponseType(typeof(ExecutorServiceDto), 200)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetExecutorServiceByIdQuery() { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddExecutorServiceDto addExecutorServiceDto)
        {
            var command = new AddExecutorServiceQuery() { AddExecutorServiceDto = addExecutorServiceDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateExecutorServiceDto updateExecutorServiceDto)
        {
            var command = new UpdateExecutorServiceQuery() { UpdateExecutorServiceDto = updateExecutorServiceDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveExecutorServiceQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}