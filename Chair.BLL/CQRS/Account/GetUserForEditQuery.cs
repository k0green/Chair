using Chair.BLL.Dto.Account;
using MediatR;

namespace Chair.BLL.CQRS.Account
{
    public class GetUserForEditQuery : IRequest<EditUserDto>
    {
        public string Id { get; set; }
    }
}