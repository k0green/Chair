﻿using MediatR;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Extension.Models;
using opr_lib;

namespace Chair.BLL.CQRS.ExecutorService
{
    public class GetOptimizeServiceQuery : IRequest<ExecutorServiceDto>
    {
        public Guid ServiceTypeId { get; set; }
        public List<Condition> Conditions { get; set; }
        public FilterModel FilterModel { get; set; }
    }
}
