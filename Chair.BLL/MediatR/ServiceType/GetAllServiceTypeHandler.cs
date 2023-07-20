using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class GetAllServiceTypeHandler : IRequestHandler<GetAllServiceTypesQuery, List<ServiceTypeDto>>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public GetAllServiceTypeHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<List<ServiceTypeDto>> Handle(GetAllServiceTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await _serviceTypeBusinessLogic.GetAllServiceTypes();

            return result;
        }
    }
}
