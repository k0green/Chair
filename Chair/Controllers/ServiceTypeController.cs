using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("service-types")]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutorServiceController> _logger;

        public ServiceTypeController(IMediator mediator,
            ILogger<ExecutorServiceController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(List<ServiceTypeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllServiceTypesQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-by-id/{id:guid}")]
        [ProducesResponseType(typeof(ServiceTypeDto), 200)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetServiceTypeByIdQuery() { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddServiceTypeDto addServiceTypeDto)
        {
            var command = new AddServiceTypeQuery() { AddServiceTypeDto = addServiceTypeDto };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(ServiceTypeDto serviceType)
        {
            var command = new UpdateServiceTypeQuery() { ServiceTypeDto = serviceType };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveServiceTypeQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}