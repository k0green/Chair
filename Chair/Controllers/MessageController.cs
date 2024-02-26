using Chair.BLL.Commons;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.CQRS.Message;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.Message;
using Chair.BLL.Dto.ServiceType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chair.Controllers
{
    [ApiController]
    [Route("message")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MessageController> _logger;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageController(IMediator mediator,
            ILogger<MessageController> logger,
            IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add([FromBody] AddMessageDto dto)
        {
            var query = new AddMessageQuery() { AddMessageDto = dto };
            var result = await _mediator.Send(query);
            
            //await _hubContext.Clients.User(dto.RecipientId).SendAsync("ReceiveMessage", dto.Text);
            await _hubContext.Clients.Users(dto.RecipientId, dto.SenderId).SendAsync("ReceiveMessage", result);
            
            return Ok(result);
        }

        [HttpGet]
        [Route("mark-as-read/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
        {
            var query = new MarkAsReadMessageQuery() { ChatId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("edit-text")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EditTextMessage(LookupDto lookupDto)
        {
            var command = new EditMessageTextQuery() { LookupDto = lookupDto };
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        [Route("remove/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveById([FromRoute] Guid id)
        {
            var command = new RemoveMessageQuery() { Id = id };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}