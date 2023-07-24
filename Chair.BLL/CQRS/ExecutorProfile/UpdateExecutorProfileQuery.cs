using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.CQRS.ServiceType
{
    public class UpdateExecutorProfileQuery : IRequest<Unit>
    {
        public UpdateExecutorProfileDto UpdateExecutorProfileDto { get; set; }
    }
}
