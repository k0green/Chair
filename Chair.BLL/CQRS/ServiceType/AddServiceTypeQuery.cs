using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.CQRS.ServiceType
{
    public class UpdateServiceTypeQuery : IRequest<Unit>
    {
        public ServiceTypeDto ServiceTypeDto { get; set; }
    }
}
