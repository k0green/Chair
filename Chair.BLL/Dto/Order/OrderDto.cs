using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Contacts;

namespace Chair.BLL.Dto.Order
{
    public class OrderDto : BaseDto
    {
        public Guid ExecutorServiceId { get; set; }
        public string ExecutorProfileName { get; set; }
        public string ExecutorName { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime StarDate { get; set; }
        public int Day => StarDate.Day;
        public int Month => StarDate.Month;
        public DateTime Duration { get; set; }
        public string ExecutorComment { get; set; }
        public string ClientComment { get; set; }
        public bool ExecutorApprove { get; set; }
        public bool ClientApprove { get; set; }
        public decimal? Price { get; set; }
        public string ServiceTypeName { get; set; }
    }
}
