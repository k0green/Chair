using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetOptimizeServiceHandler : IRequestHandler<GetOptimizeServiceQuery, ExecutorServiceDto>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetOptimizeServiceHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<ExecutorServiceDto> Handle(GetOptimizeServiceQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetOptimizeService(request.FilterModel, request.ServiceTypeId, request.Conditions);

            return result;
        }
    }
}
