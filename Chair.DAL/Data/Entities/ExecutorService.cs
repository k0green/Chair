namespace Chair.DAL.Data.Entities
{
    public class ExecutorService : BaseEntity
    {
        public Guid ServiceTypeId { get; set; }
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Duration { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool IsDeleted { get; set; }
        public ServiceType ServiceType { get; set; }
        public ExecutorProfile Executor { get; set; }
        public ICollection<ProductFile<ExecutorService>> Images { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
