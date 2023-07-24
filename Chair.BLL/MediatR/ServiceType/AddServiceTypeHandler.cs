using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class AddServiceTypeHandler : IRequestHandler<AddServiceTypeQuery, Guid>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public AddServiceTypeHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<Guid> Handle(AddServiceTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _serviceTypeBusinessLogic.AddAsync(request.AddServiceTypeDto);

            return result;
        }
    }
}
