using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorPromotion
{
    public class UpdateExecutorPromotionDto : BaseDto
    {
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public List<Guid> PhotoIds { get; set; }
        public List<Guid> RemovePhotoIds { get; set; }
    }
}
