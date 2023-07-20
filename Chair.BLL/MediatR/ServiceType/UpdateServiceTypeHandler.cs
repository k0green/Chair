using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class UpdateServiceTypeHandler : IRequestHandler<UpdateServiceTypeQuery, Unit>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public UpdateServiceTypeHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<Unit> Handle(UpdateServiceTypeQuery request, CancellationToken cancellationToken)
        {
            await _serviceTypeBusinessLogic.UpdateAsync(request.ServiceTypeDto);

            return Unit.Value;
        }
    }
}
