namespace Chair.DAL.Data.Entities
{
    public class ExecutorPromotion : BaseEntity
    {
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public ExecutorProfile Executor { get; set; }
        public ICollection<ProductFile<ExecutorPromotion>> Images { get; set; }
    }
}
