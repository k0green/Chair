using Chair.BLL.Dto.Contacts;
using Chair.DAL.Data.Entities;

namespace Chair.BLL.Dto.Order
{
    public class AddOrderDto
    {
        public Guid ExecutorServiceId { get; set; }
        public string? ClientId { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime Duration { get; set; }
        public string? ExecutorComment { get; set; }
        public string? ClientComment { get; set; }
        public bool ExecutorApprove => false;
        public bool ClientApprove => false;
        public decimal? Price { get; set; }
    }
}
