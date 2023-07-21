using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.ExecutorService
{
    public class GetAllServicesByExecutorIdHandler : IRequestHandler<GetAllServicesByExecutorIdQuery, List<ExecutorServiceDto>>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetAllServicesByExecutorIdHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<List<ExecutorServiceDto>> Handle(GetAllServicesByExecutorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetAllServicesByExecutorId(request.ExecutorId);

            return result;
        }
    }
}
