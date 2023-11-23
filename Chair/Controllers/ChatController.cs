using Chair.BLL.Commons;
using Chair.BLL.CQRS.Chat;
using Chair.BLL.CQRS.Order;
using Chair.BLL.Dto.Chat;
using Chair.BLL.Dto.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chair.Controllers
{
    [ApiController]
    [Route("chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChatController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ChatController(IMediator mediator,
        IHubContext<NotificationHub> hubContext,
        ILogger<ChatController> logger)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("{profileId}")]
        [ProducesResponseType(typeof(ChatDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatForProfile([FromRoute] Guid profileId)
        {
            var query = new GetChatForProfileQuery() { ProfileId = profileId};
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<ChatDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllChatForCurrentUser()
        {
            var query = new GetAllChatForCurrentUserQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            var command = new RemoveOrderQuery() { Id = id };
            await _mediator.Send(command);
            await _hubContext.Clients.All.SendAsync("ReceiveReviewNotification", "отзыв удален");
            return NoContent();
        }
    }
}