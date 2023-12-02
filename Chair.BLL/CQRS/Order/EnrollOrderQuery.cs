﻿using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class EnrollOrderQuery : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
    }
}