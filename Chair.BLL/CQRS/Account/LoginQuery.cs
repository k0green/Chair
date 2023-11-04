using Chair.BLL.Dto.Account;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class LoginQuery : IRequest<string>
    {
        public LoginDto LoginDto { get; set; }
    }
}
