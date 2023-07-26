using Microsoft.AspNetCore.Identity;

namespace Chair.DAL.Data.Entities
{
    public class User : IdentityUser
    {
        public string AccountName { get; set; }
        public ICollection<ExecutorService> ExecutorServices { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
