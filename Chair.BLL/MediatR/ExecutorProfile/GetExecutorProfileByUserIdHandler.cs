using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorProfile
{
    public class GetExecutorProfileByUserIdHandler : IRequestHandler<GetExecutorProfileByUserIdQuery, ExecutorProfileDto>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public GetExecutorProfileByUserIdHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<ExecutorProfileDto> Handle(GetExecutorProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorProfileBusinessLogic.GetExecutorProfileByUserId();

            return result;
        }
    }
}
