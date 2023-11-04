using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class RegisterHandler : IRequestHandler<RegisterQuery, string>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public RegisterHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<string> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            return await _accountBusinessLogic.Register(request.RegisterDto);
        }
    }
}
