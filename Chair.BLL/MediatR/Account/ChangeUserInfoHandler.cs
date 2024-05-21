using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.Account;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class ChangeUserInfoHandler : IRequestHandler<ChangeUserInfoQuery, Unit>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public ChangeUserInfoHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<Unit> Handle(ChangeUserInfoQuery request, CancellationToken cancellationToken)
        {
            await _accountBusinessLogic.ChangeUserInfo(request.Dto);
            return Unit.Value;
        }
    }
}
