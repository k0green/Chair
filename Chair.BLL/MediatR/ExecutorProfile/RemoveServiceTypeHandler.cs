using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class RemoveExecutorProfileHandler : IRequestHandler<RemoveExecutorProfileQuery, Unit>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public RemoveExecutorProfileHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveExecutorProfileQuery request, CancellationToken cancellationToken)
        {
            await _executorProfileBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
