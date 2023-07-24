using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class RegisterHandler : IRequestHandler<RegisterQuery, Unit>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public RegisterHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<Unit> Handle(RegisterQuery request, CancellationToken cancellationToken)
        {
            await _accountBusinessLogic.Register(request.RegisterDto);

            return Unit.Value;
        }
    }
}
