using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class RemoveExecutorServiceHandler : IRequestHandler<RemoveExecutorServiceQuery, Unit>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public RemoveExecutorServiceHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveExecutorServiceQuery request, CancellationToken cancellationToken)
        {
            await _executorServiceBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
