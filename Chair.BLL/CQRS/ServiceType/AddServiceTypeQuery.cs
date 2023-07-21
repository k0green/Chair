using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.CQRS.ServiceType
{
    public class AddServiceTypeQuery : IRequest<Guid>
    {
        public AddServiceTypeDto AddServiceTypeDto { get; set; }
    }
}
