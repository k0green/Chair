using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetAllServicesNamesHandler : IRequestHandler<GetAllServicesNamesQuery, List<LookupDto>>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetAllServicesNamesHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<List<LookupDto>> Handle(GetAllServicesNamesQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetAllServicesNames();

            return result;
        }
    }
}
