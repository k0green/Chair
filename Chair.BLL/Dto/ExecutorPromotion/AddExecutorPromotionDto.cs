namespace Chair.BLL.Dto.ExecutorPromotion
{
    public class AddExecutorPromotionDto
    {
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public List<Guid> PhotoIds { get; set; }
    }
}
