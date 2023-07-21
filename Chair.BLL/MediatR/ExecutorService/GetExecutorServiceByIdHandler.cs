﻿using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.ServiceType
{
    public class GetExecutorServiceByIdHandler : IRequestHandler<GetExecutorServiceByIdQuery, ExecutorServiceDto>
    {
        private readonly IExecutorServiceBusinessLogic _executorServiceBusinessLogic;

        public GetExecutorServiceByIdHandler(IExecutorServiceBusinessLogic executorServiceBusinessLogic)
        {
            _executorServiceBusinessLogic = executorServiceBusinessLogic;
        }

        public async Task<ExecutorServiceDto> Handle(GetExecutorServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _executorServiceBusinessLogic.GetExecutorServiceById(request.Id);

            return result;
        }
    }
}