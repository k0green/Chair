using Chair.DAL.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chair.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ExecutorService> ExecutorServices { get; set; }
        public DbSet<ExecutorProfile> ExecutorProfiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExecutorService>()
                .HasOne(es => es.ServiceType)
                .WithMany(st => st.ExecutorServices)
                .HasForeignKey(es => es.ServiceTypeId);

            modelBuilder.Entity<ExecutorService>()
                .HasOne(es => es.Executor)
                .WithMany()
                .HasForeignKey(es => es.ExecutorId);

            modelBuilder.Entity<ExecutorProfile>()
                .HasOne(es => es.User)
                .WithMany()
                .HasForeignKey(es => es.UserId);
        }
    }
}
