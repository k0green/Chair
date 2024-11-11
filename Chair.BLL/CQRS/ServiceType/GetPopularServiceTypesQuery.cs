using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;
using Chair.DAL.Extension.Models;

namespace Chair.BLL.CQRS.ServiceType
{
    public class GetPopularServiceTypesQuery : IRequest<List<ServiceTypeDto>>
    {
        public Guid? ParentId { get; set; } 
        public FilterModel Filter { get; set; }
    }
}
