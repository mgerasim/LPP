using LPP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LPP.DAL.Context
{
    public class LPPContext : DbContext
    {
        public LPPContext(DbContextOptions<LPPContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageDelivery> MessageDeliveries { get; set; }

        public DbSet<MessageReaction> MessageReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(this.Users));

            modelBuilder.Entity<User>().HasIndex(u => u.TelegramId).IsUnique();

            modelBuilder.Entity<MessageReaction>()
                .ToTable(nameof(this.MessageReactions));

            modelBuilder.Entity<MessageReaction>()
                .HasOne<Message>()
                .WithMany()
                .HasForeignKey(p => p.MessageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MessageReaction>().HasIndex(x => new
            {
                x.UserId,
                x.MessageId,
            }).IsUnique();

            modelBuilder.Entity<Message>()
                .ToTable(nameof(this.Messages));

            modelBuilder.Entity<MessageDelivery>()
                .ToTable(nameof(this.MessageDeliveries));

            modelBuilder.Entity<MessageDelivery>()
                .HasOne<Message>()
                .WithMany()
                .HasForeignKey(p => p.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
