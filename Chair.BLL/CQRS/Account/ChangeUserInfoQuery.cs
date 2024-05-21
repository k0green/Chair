using Chair.BLL.Dto.Account;
using MediatR;

namespace Chair.BLL.CQRS.Account
{
    public class ChangeUserInfoQuery : IRequest<Unit>
    {
        public EditUserDto Dto { get; set; }
    }
}