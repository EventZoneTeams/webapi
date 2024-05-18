using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories
{
    public class StudentEventForumDbContext : IdentityDbContext<User, Role, int>
    {
        public StudentEventForumDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventComment> EventComments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<EventOrder> EventOrders { get; set; }
        public DbSet<EventOrderDetail> EventOrderDetails { get; set; }
        public DbSet<EventPackage> EventPackages { get; set; }
        public DbSet<EventProduct> EventProducts { get; set; }
        public DbSet<ProductInPackage> ProductInPackages { get; set; }
        public DbSet<OrderTransaction> OrderTransactions { get; set; }
        public DbSet<EventFeedback> EventFeedbacks { get; set; }
        public DbSet<EventImage> EventImages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            // Configure entity relationships
            modelBuilder.Entity<ProductInPackage>()
                .HasKey(pp => new { pp.ProductId, pp.PackageId });


            modelBuilder.Entity<EventComment>()
                .HasKey(ec => ec.Id);
            modelBuilder.Entity<EventComment>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventComments)
                .HasForeignKey(ec => ec.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasKey(pc => pc.Id);
            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostComments)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}