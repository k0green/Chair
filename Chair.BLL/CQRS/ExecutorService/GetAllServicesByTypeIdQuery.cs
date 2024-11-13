using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesByTypeIdQuery : IRequest<(List<GroupExecutorServiceDto>, int)>
    {
        public Guid? TypeId { get; set; }
        public FilterModelWithPeriods Filter { get; set; }
    }
}
