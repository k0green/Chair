using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetAllServicesNamesByUserIdHandler : IRequestHandler<GetAllServicesNamesByUserIdQuery, List<LookupDto>>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetAllServicesNamesByUserIdHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<List<LookupDto>> Handle(GetAllServicesNamesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetAllServicesNamesByUserId();

            return result;
        }
    }
}
