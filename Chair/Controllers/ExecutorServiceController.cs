using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Extension.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using opr_lib;

namespace Chair.Controllers
{
    [ApiController]
    [Route("executor-service")]
    [Authorize]
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
        [Route("type/{typeId:guid}")]
        [ProducesResponseType(typeof(List<ExecutorServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByTypeId([FromRoute] Guid typeId)
        {
            var query = new GetAllServicesByTypeIdQuery() { TypeId = typeId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("executor-services/lookup")]
        [ProducesResponseType(typeof(List<ExecutorServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServicesNamesByUserId()
        {
            var query = new GetAllServicesNamesByUserIdQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("executor-services/lookup/all")]
        [ProducesResponseType(typeof(List<ExecutorServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServicesNames()
        {
            var query = new GetAllServicesNamesQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ExecutorServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServices(FilterModel filter)
        {
            var query = new GetAllServicesQuery() { Filter = filter};
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
        public async Task<IActionResult> Add([FromBody] AddExecutorServiceDto addExecutorServiceDto)
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
        
        

        [HttpPost]
        [AllowAnonymous]
        [Route("optimize-service")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] GetOptimizeServiceDto dto)
        {
            var command = new GetOptimizeServiceQuery()
            {
                FilterModel = dto.FilterModel,
                Conditions = dto.Conditions,
                ServiceTypeId = dto.ServiceTypeId,
            };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}