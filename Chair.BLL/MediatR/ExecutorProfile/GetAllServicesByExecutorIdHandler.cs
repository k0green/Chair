using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.CQRS.ExecutorProfile;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorProfile
{
    public class GetAllProfilesByServiceTypeIdHandler : IRequestHandler<GetAllProfilesByServiceTypeIdQuery, List<ExecutorProfileDto>>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public GetAllProfilesByServiceTypeIdHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<List<ExecutorProfileDto>> Handle(GetAllProfilesByServiceTypeIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorProfileBusinessLogic.GetAllProfilesByServiceTypeId(request.ServiceTypeId);

            return result;
        }
    }
}
