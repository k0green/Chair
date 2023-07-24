using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class GetServiceTypeByIdHandler : IRequestHandler<GetServiceTypeByIdQuery, ServiceTypeDto>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public GetServiceTypeByIdHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<ServiceTypeDto> Handle(GetServiceTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _serviceTypeBusinessLogic.GetServiceTypeById(request.Id);

            return result;
        }
    }
}
