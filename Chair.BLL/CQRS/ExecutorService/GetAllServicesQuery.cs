using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension.Models;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesQuery : IRequest<List<GroupExecutorServiceDto>>
    {
        public FilterModelWithPeriods Filter { get; set; }
    }
}
