using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.Order
{
    public class UpdateOrderDto : BaseDto
    {
        public Guid ExecutorServiceId { get; set; }
        public string? ClientId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime Duration { get; set; }
        public string? ExecutorComment { get; set; }
        public string? ClientComment { get; set; }
        public bool ExecutorApprove { get; set; }
        public bool ClientApprove { get; set; }
        public decimal? Price { get; set; }
    }
}
