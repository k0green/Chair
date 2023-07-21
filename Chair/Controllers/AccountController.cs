using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var query = new LoginQuery() { LoginDto = dto };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var query = new RegisterQuery() { RegisterDto = dto };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutQuery();
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}