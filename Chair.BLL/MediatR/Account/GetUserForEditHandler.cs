using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.CQRS.Account;
using Chair.BLL.Dto.Account;
using MediatR;

namespace Chair.BLL.MediatR.Account
{
    public class GetUserForEditHandler : IRequestHandler<GetUserForEditQuery, EditUserDto>
    {
        private readonly IAccountBusinessLogic _accountBusinessLogic;

        public GetUserForEditHandler(IAccountBusinessLogic accountBusinessLogic)
        {
            _accountBusinessLogic = accountBusinessLogic;
        }

        public async Task<EditUserDto> Handle(GetUserForEditQuery request, CancellationToken cancellationToken)
        {
            return await _accountBusinessLogic.GetUserForEdit(request.Id);
        }
    }
}
