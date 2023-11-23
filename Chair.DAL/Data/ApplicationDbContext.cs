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
        public DbSet<Image> Images { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

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

            modelBuilder.Entity<Image>()
                .HasOne(i => i.ExecutorService)
                .WithMany(es => es.Images)
                .HasForeignKey(es => es.ObjectId);

            modelBuilder.Entity<Contact>()
                .HasOne(es => es.Executor)
                .WithMany(st => st.Contacts)
                .HasForeignKey(es => es.ExecutorProfileId);  
            
            modelBuilder.Entity<Review>()
                .HasOne(es => es.ExecutorService)
                .WithMany(st => st.Reviews)
                .HasForeignKey(es => es.ExecutorServiceId);

            modelBuilder.Entity<Order>()
                .HasOne(es => es.ExecutorService)
                .WithMany(st => st.Orders)
                .HasForeignKey(es => es.ExecutorServiceId);

            modelBuilder.Entity<Order>()
                .HasOne(es => es.User)
                .WithMany(st => st.Orders)
                .HasForeignKey(es => es.ClientId);

            modelBuilder.Entity<Message>()
                .HasOne(es => es.Recipient)
                .WithMany(st => st.RecipientChats)
                .HasForeignKey(es => es.RecipientId);

            modelBuilder.Entity<Message>()
                .HasOne(es => es.Sender)
                .WithMany(st => st.SenderChats)
                .HasForeignKey(es => es.SenderId);

            modelBuilder.Entity<Message>()
                .HasOne(es => es.Chat)
                .WithMany(st => st.Messages)
                .HasForeignKey(es => es.ChatId);
        }
    }
}
