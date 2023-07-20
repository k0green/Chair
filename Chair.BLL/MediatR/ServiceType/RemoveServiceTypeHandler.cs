using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class RemoveServiceTypeHandler : IRequestHandler<RemoveServiceTypeQuery, Unit>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public RemoveServiceTypeHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveServiceTypeQuery request, CancellationToken cancellationToken)
        {
            await _serviceTypeBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
