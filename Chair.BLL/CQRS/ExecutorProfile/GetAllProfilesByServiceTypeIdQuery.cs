using MediatR;
using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.CQRS.ExecutorProfile
{
    public class GetAllProfilesByServiceTypeIdQuery : IRequest<List<ExecutorProfileDto>>
    {
        public Guid ServiceTypeId { get; set; }
    }
}
