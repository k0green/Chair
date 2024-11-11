namespace Chair.DAL.Data.Entities
{
    public class ServiceType : BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid? ParentId { get; set; }
        public ServiceType Parent { get; set; }

        public ICollection<ExecutorService> ExecutorServices { get; set; }
    }
}
