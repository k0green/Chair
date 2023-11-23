namespace Chair.DAL.Data.Entities
{
    public class Chat : BaseEntity
    {
        public bool IsDeleted { get; set; }
        
        public ICollection<Message> Messages { get; set; }
    }
}
