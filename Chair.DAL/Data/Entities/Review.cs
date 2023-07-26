namespace Chair.DAL.Data.Entities
{
    public class Review : BaseEntity
    {
        public Guid ExecutorServiceId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsModify { get; set; }

        public ExecutorService ExecutorService { get; set; }
    }
}
