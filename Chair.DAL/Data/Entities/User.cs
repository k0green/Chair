using Microsoft.AspNetCore.Identity;

namespace Chair.DAL.Data.Entities
{
    public class User : IdentityUser
    {
        public ICollection<ExecutorService> ExecutorServices { get; set; }
    }
}
