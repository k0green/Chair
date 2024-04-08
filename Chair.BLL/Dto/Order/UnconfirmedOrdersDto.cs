namespace Chair.BLL.Dto.Order;

public class UnconfirmedOrdersDto
{
    public List<OrderDto> ByMaster { get; set; }
    public List<OrderDto> ByClient { get; set; }
    public List<OrderDto> ForToday { get; set; }
    public List<OrderDto> ForWeek { get; set; }
}