﻿using Chair.BLL.Dto.Order;
using MediatR;

namespace Chair.BLL.CQRS.Order
{
    public class GetUnconfirmedOrdersForClientQuery : IRequest<List<OrderDto>>
    {
    }
}
