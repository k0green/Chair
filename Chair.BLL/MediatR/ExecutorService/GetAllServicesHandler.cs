using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, List<GroupExecutorServiceDto>>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetAllServicesHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<List<GroupExecutorServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetAllServices();

            return result;
        }
    }
}
