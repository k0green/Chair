using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorProfile
{
    public class GetExecutorProfileByIdHandler : IRequestHandler<GetExecutorProfileByIdQuery, ExecutorProfileDto>
    {
        private readonly IExecutorProfileBusinessLogic _executorProfileBusinessLogic;

        public GetExecutorProfileByIdHandler(IExecutorProfileBusinessLogic executorProfileBusinessLogic)
        {
            _executorProfileBusinessLogic = executorProfileBusinessLogic;
        }

        public async Task<ExecutorProfileDto> Handle(GetExecutorProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorProfileBusinessLogic.GetExecutorProfileById(request.Id);

            return result;
        }
    }
}
