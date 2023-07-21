using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class UpdateServiceTypeQuery : IRequest<Unit>
    {
        public ServiceTypeDto ServiceTypeDto { get; set; }
    }
}
