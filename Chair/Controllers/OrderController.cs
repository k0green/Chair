using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator,
            ILogger<OrderController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{executorServiceId}")]
        [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid executorServiceId)
        {
            var query = new GetAllOrdersByServiceIdQuery() { ExecutorServiceId = executorServiceId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-by-id/{id:guid}")]
        [ProducesResponseType(typeof(OrderDto), 200)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetOrderByIdQuery() { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] List<AddOrderDto> addOrderDtos)
        {
            var command = new AddOrderQuery() { AddOrderDtos = addOrderDtos };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateOrderDto updateOrderDto)
        {
            var command = new UpdateOrderQuery() { UpdateOrderDto = updateOrderDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveOrderQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}