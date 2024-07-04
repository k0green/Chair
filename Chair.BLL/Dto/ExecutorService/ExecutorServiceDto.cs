using System.Text.Json.Serialization;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Minio;
using Chair.BLL.Dto.Order;

namespace Chair.BLL.Dto.ExecutorService
{
    public class ExecutorServiceDto : BaseDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public Guid ExecutorId { get; set; }
        public string ExecutorName { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public int AvailableSlots => Orders.Count(x => string.IsNullOrEmpty(x.ClientId) && x.StarDate >= DateTime.Now);
        [JsonIgnore]public List<OrderDto> Orders { get; set; }
        public DateTime Duration { get; set; }
        public Place Place { get; set; }
        public List<ShortMinioFileDto> Photos { get; set; }
    }
}
