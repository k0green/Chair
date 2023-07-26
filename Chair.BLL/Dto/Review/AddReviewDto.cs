namespace Chair.BLL.Dto.Review
{
    public class AddReviewDto
    {
        public Guid ExecutorServiceId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsModify => false;
    }
}
