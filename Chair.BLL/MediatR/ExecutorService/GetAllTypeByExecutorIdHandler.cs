using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetAllTypeByExecutorIdHandler : IRequestHandler<GetAllServicesByTypeIdQuery, List<GroupExecutorServiceDto>>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetAllTypeByExecutorIdHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<List<GroupExecutorServiceDto>> Handle(GetAllServicesByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetAllServicesByTypeId(request.TypeId);

            return result;
        }
    }
}
