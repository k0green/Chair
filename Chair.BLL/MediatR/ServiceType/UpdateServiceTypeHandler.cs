using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.CQRS.ServiceType;
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
