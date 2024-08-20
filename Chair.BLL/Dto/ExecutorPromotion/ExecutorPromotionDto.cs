using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Minio;

namespace Chair.BLL.Dto.ExecutorPromotion
{
    public class ExecutorPromotionDto : BaseDto
    {
        public Guid ExecutorId { get; set; }
        public string UserId { get; set; }
        public string ExecutorName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public List<ShortMinioFileDto> Photos { get; set; }
    }
}
