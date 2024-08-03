using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class StudentEventForumDbContext : IdentityDbContext<User, Role, Guid>
    {
        public StudentEventForumDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventComment> EventComments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<EventOrder> EventOrders { get; set; }
        public DbSet<EventOrderDetail> EventOrderDetails { get; set; }
        public DbSet<EventPackage> EventPackages { get; set; }
        public DbSet<EventProduct> EventProducts { get; set; }
        public DbSet<ProductInPackage> ProductInPackages { get; set; }
        public DbSet<EventFeedback> EventFeedbacks { get; set; }
        public DbSet<EventImage> EventImages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<EventCampaign> EventCampaigns { get; set; }
        public DbSet<EventDonation> EventDonations { get; set; }

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

            modelBuilder.Entity<ProductInPackage>()
                .HasOne(pp => pp.EventProduct)
                .WithMany(p => p.ProductsInPackage)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductInPackage>()
                .HasOne(pp => pp.EventPackage)
                .WithMany(p => p.ProductsInPackage)
                .HasForeignKey(pp => pp.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<EventProduct>()
      .HasMany(e => e.ProductImages)
      .WithOne(p => p.EventProduct)
      .OnDelete(DeleteBehavior.Cascade);
        }
    }
}