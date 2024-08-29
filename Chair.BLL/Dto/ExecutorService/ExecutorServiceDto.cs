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
        public string UserId { get; set; }
        public string ExecutorName { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public decimal Price => Orders.Any() ? decimal.Round(Orders.Average(x => x.Price ?? 0), 2) : 0;
        public bool IsDeleted { get; set; }
        public int AvailableSlots => Orders.Count(x => string.IsNullOrEmpty(x.ClientId) && x.StarDate >= DateTime.Now);
        public int SuccessOrdersAmount => Orders.Count(x => !string.IsNullOrEmpty(x.ClientId)
                                                            && x.StarDate <= DateTime.Now
                                                            && x is { ClientApprove: true, ExecutorApprove: true });
        [JsonIgnore]public List<OrderDto> Orders { get; set; }
        public DateTime Duration => Orders.Any() ? DateTime.MinValue + TimeSpan.FromMinutes(Orders.Select(service =>
                (service.Duration - service.StarDate).TotalMinutes).Average()) : DateTime.MinValue;
        public Place Place { get; set; }
        public bool HasDiscount => Orders.Any(x => x.DiscountPrice.HasValue && x.StarDate >= DateTime.Now);
        public bool HasPromotions { get; set; }
        public List<ShortMinioFileDto> Photos { get; set; }
    }
}
