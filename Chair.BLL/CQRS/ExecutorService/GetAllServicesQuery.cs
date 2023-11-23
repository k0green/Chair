using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Extension.Models;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetAllServicesQuery : IRequest<List<GroupExecutorServiceDto>>
    {
        public FilterModel Filter { get; set; }
    }
}
