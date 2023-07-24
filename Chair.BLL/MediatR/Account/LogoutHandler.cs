using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class LogoutHandler : IRequestHandler<LogoutQuery, Unit>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public LogoutHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<Unit> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            await _accountBusinessLogic.Logout();

            return Unit.Value;
        }
    }
}
