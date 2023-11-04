using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;
using Chair.BLL.Dto.Account;

namespace Chair.BLL.CQRS.ServiceType
{
    public class RegisterQuery : IRequest<string>
    {
        public RegisterDto RegisterDto { get; set; }
    }
}
