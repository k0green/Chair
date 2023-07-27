using Chair.BLL.Commons;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chair.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderController(IMediator mediator,
        IHubContext<NotificationHub> hubContext,
        ILogger<OrderController> logger)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
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
        public async Task<IActionResult> AddMany([FromBody] List<AddOrderDto> addOrderDtos)
        {
            var command = new AddOrderQuery() { AddOrderDtos = addOrderDtos };
            var result = await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveReviewNotification", "Новый отзыв добавлен");
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateOrderDto updateOrderDto)
        {
            var command = new UpdateOrderQuery() { UpdateOrderDto = updateOrderDto };
            await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveReviewNotification", "отзыв обновлен");
            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveOrderQuery() { Id = id };
            await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveReviewNotification", "отзыв удален");
            return NoContent();
        }

        [HttpPost]
        [Route("approve")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Approve([FromBody] Guid orderId, bool isExecutor)
        {
            var command = new ApproveOrderQuery() { OrderId = orderId, IsExecutor = isExecutor};
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("cancel/{orderId:guid}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Cancel([FromRoute] Guid orderId)
        {
            var command = new CancelOrderQuery() { OrderId = orderId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}