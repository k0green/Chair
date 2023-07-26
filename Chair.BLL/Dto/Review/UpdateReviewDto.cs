using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.Review
{
    public class UpdateReviewDto : BaseDto
    {
        public Guid ExecutorServiceId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsModify => true;
    }
}
