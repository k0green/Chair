using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class UpdateExecutorServiceHandler : IRequestHandler<UpdateExecutorServiceQuery, Unit>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public UpdateExecutorServiceHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateExecutorServiceQuery request, CancellationToken cancellationToken)
        {
            await _executorServiceBusinessLogic.UpdateAsync(request.UpdateExecutorServiceDto);

            return Unit.Value;
        }
    }
}
