namespace Chair.DAL.Data.Entities
{
    public class ExecutorProfile : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string ImageURL { get; set; }

        public User User { get; set; }
        public ICollection<Contact> Contacts { get; set; }

    }
}
