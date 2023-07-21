using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class AddExecutorServiceHandler : IRequestHandler<AddExecutorServiceQuery, Guid>
    {
        private readonly IExecutorServiceBusinessLogic _executorService;

        public AddExecutorServiceHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorService = executorServiceBusinessLogic;
        }

        public async Task<Guid> Handle(AddExecutorServiceQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorService.AddAsync(request.AddExecutorServiceDto);

            return result;
        }
    }
}
