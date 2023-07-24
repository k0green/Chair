using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorProfile
{
    public class UpdateExecutorProfileHandler : IRequestHandler<UpdateExecutorProfileQuery, Unit>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public UpdateExecutorProfileHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateExecutorProfileQuery request, CancellationToken cancellationToken)
        {
            await _executorProfileBusinessLogic.UpdateAsync(request.UpdateExecutorProfileDto);

            return Unit.Value;
        }
    }
}
