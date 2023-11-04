using Chair.BLL.BusinessLogic.Account;
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
        private readonly UserInfo _userInfo;

        public AccountController(IMediator mediator,
            ILogger<AccountController> logger,
            UserInfo userInfo)
        {
            _logger = logger;
            _mediator = mediator;
            _userInfo = userInfo;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var query = new LoginQuery() { LoginDto = dto };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        [HttpGet]
        [Route("user-info")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetUserInfo([FromQuery] string email)
        {
            var result = await _userInfo.GetCurrentUserInfoByEmail(email);

            return Ok(result);
        }

        [HttpGet]
        [Route("user-id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetUserId()
        {
            return Ok(_userInfo.GetCurrentUserInfo());
        }
    }
}