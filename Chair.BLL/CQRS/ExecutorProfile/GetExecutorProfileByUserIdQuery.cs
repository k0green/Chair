﻿using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.BLL.Dto.ServiceType;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.CQRS.ServiceType
{
    public class GetExecutorProfileByUserIdQuery : IRequest<ExecutorProfileDto>
    {
    }
}