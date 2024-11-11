using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class GetPopularServiceTypesHandler : IRequestHandler<GetPopularServiceTypesQuery, List<ServiceTypeDto>>
    {
        private readonly IServiceTypeBusinessLogic _serviceTypeBusinessLogic;

        public GetPopularServiceTypesHandler(IServiceTypeBusinessLogic serviceTypeBusinessLogic)
        {
            _serviceTypeBusinessLogic = serviceTypeBusinessLogic;
        }

        public async Task<List<ServiceTypeDto>> Handle(GetPopularServiceTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await _serviceTypeBusinessLogic.GetPopularServiceTypes(request.Filter, request.ParentId);

            return result;
        }
    }
}
