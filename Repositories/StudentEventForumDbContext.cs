using EventZone.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventZone.Repositories
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
        public DbSet<EventTicket> EventTickets { get; set; }
        public DbSet<BookedTicket> BookedTickets { get; set; }

        // EventBoard
        public DbSet<EventBoard> EventBoards { get; set; }
        public DbSet<EventBoardColumn> EventBoardColumns { get; set; }
        public DbSet<EventBoardTask> EventBoardTasks { get; set; }
        public DbSet<EventBoardLabel> EventBoardLabels { get; set; }
        public DbSet<EventBoardTaskLabel> EventBoardTaskLabels { get; set; }
        public DbSet<EventBoardTaskLabelAssignment> EventBoardTaskLabelAssignments { get; set; }
        public DbSet<EventBoardLabelAssignment> EventBoardLabelAssignments { get; set; }
        public DbSet<EventBoardMember> EventBoardMembers { get; set; }

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

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);  // Assuming Id is the primary key from BaseEntity

                // Configure ReceiverId as a foreign key
                entity.HasOne<User>()
                    .WithMany()  // Assuming no navigation property in User entity for this relationship
                    .HasForeignKey(e => e.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict) // Optional: specify the delete behavior
                    .IsRequired(false);  // ReceiverId is nullable
            });

            /**
             * EventBoard relationships
             *            
             */
            // EventBoard -> Event
            modelBuilder.Entity<EventBoard>()
                .HasOne(e => e.Event)
                .WithMany(e => e.EventBoards)
                .HasForeignKey(e => e.EventId);

            // EventBoard -> Leader (One-to-One Relationship)
            modelBuilder.Entity<EventBoard>()
                .HasOne(e => e.Leader)
                .WithMany()
                .HasForeignKey(e => e.LeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // EventBoard -> Members (Many-to-Many Relationship via EventBoardMember)
            modelBuilder.Entity<EventBoardMember>()
                .HasKey(ebm => new { ebm.EventBoardId, ebm.UserId }); // Composite primary key

            modelBuilder.Entity<EventBoardMember>()
                .HasOne(ebm => ebm.EventBoard)
                .WithMany(eb => eb.EventBoardMembers)
                .HasForeignKey(ebm => ebm.EventBoardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventBoardMember>()
                .HasOne(ebm => ebm.User)
                .WithMany(u => u.EventBoardMembers)
                .HasForeignKey(ebm => ebm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // EventBoardLabel -> Event
            modelBuilder.Entity<EventBoardLabel>()
                .HasOne(l => l.Event)
                .WithMany(e => e.EventBoardLabels)
                .HasForeignKey(l => l.EventId);

            // EventBoardColumn -> EventBoard
            modelBuilder.Entity<EventBoardColumn>()
                .HasOne(c => c.EventBoard)
                .WithMany(b => b.EventBoardColumns)
                .HasForeignKey(c => c.EventBoardId);

            // EventBoardLabelAssignment (Many-to-Many between EventBoard and EventBoardLabel)
            modelBuilder.Entity<EventBoardLabelAssignment>()
                .HasKey(e => new { e.EventBoardId, e.EventBoardLabelId });

            modelBuilder.Entity<EventBoardLabelAssignment>()
                .HasOne(e => e.EventBoard)
                .WithMany(b => b.EventBoardLabelAssignments)
                .HasForeignKey(e => e.EventBoardId);

            modelBuilder.Entity<EventBoardLabelAssignment>()
                .HasOne(e => e.EventBoardLabel)
                .WithMany(l => l.EventBoardLabelAssignments)
                .HasForeignKey(e => e.EventBoardLabelId);

            // EventBoardTask -> EventBoardColumn
            modelBuilder.Entity<EventBoardTask>()
                .HasOne(t => t.EventBoardColumn)
                .WithMany(c => c.EventBoardTasks)
                .HasForeignKey(t => t.EventBoardColumnId);

            // EventBoardTask -> User (Many-to-Many Relationship via EventBoardTaskAssignment)
            modelBuilder.Entity<EventBoardTaskAssignment>()
                .HasKey(e => new { e.EventBoardTaskId, e.UserId }); // Composite primary key

            modelBuilder.Entity<EventBoardTaskAssignment>()
                .HasOne(e => e.EventBoardTask)
                .WithMany(t => t.EventBoardTaskAssignments)
                .HasForeignKey(e => e.EventBoardTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventBoardTaskAssignment>()
                .HasOne(e => e.User)
                .WithMany(u => u.EventBoardTaskAssignments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // EventBoardTaskLabel -> Many-to-Many with EventBoardTask
            modelBuilder.Entity<EventBoardTaskLabelAssignment>()
                .HasKey(e => new { e.EventBoardTaskId, e.EventBoardTaskLabelId });

            modelBuilder.Entity<EventBoardTaskLabelAssignment>()
                .HasOne(e => e.EventBoardTask)
                .WithMany(t => t.EventBoardTaskLabelAssignments)
                .HasForeignKey(e => e.EventBoardTaskId);

            modelBuilder.Entity<EventBoardTaskLabelAssignment>()
                .HasOne(e => e.EventBoardTaskLabel)
                .WithMany(l => l.EventBoardTaskLabelAssignments)
                .HasForeignKey(e => e.EventBoardTaskLabelId);

        }
    }
}