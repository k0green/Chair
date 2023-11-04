using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class LoginHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public LoginHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await _accountBusinessLogic.Login(request.LoginDto);
        }
    }
}
