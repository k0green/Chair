using Chair.BLL.Commons;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.Review;
using Chair.BLL.Dto.Review;
using Chair.DAL.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chair.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReviewController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ReviewController(IMediator mediator,
            IHubContext<NotificationHub> hubContext,
            ILogger<ReviewController> logger)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("{executorServiceId}")]
        [ProducesResponseType(typeof(List<ReviewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByContractId([FromRoute] Guid executorServiceId)
        {
            var query = new GetAllReviewsForServiceQuery() { ExecutorServiceId = executorServiceId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] AddReviewDto addReviewDto)
        {
            var command = new AddReviewQuery() { AddReviewDto = addReviewDto };
            var result = await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Заказ обновлен");
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateReviewDto updateReviewDto)
        {
            var command = new UpdateReviewQuery() { UpdateReviewDto = updateReviewDto };
            await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Заказ обновлен");
            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveReviewQuery() { Id = id };
            await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", "Заказ обновлен");
            return NoContent();
        }
    }
}