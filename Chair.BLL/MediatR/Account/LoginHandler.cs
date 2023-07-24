using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class LoginHandler : IRequestHandler<LoginQuery, Unit>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public LoginHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<Unit> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _accountBusinessLogic.Login(request.LoginDto);

            return Unit.Value;
        }
    }
}
