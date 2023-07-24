using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorProfile
{
    public class AddExecutorProfileHandler : IRequestHandler<AddExecutorProfileQuery, Guid>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public AddExecutorProfileHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<Guid> Handle(AddExecutorProfileQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorProfileBusinessLogic.AddAsync(request.AddExecutorProfileDto);

            return result;
        }
    }
}
