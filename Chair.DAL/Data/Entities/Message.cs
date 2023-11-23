namespace Chair.DAL.Data.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEdited { get; set; }
        public bool IsRead { get; set; }
        public Guid ChatId { get; set; }
        public Guid? ReplyId { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }
        
        public User Recipient { get; set; }
        public User Sender { get; set; }
        public Chat Chat { get; set; }
        public Message Reply { get; set; }

    }
}
