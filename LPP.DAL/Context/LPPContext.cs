using LPP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LPP.DAL.Context
{
    public class LPPContext : DbContext
    {
        public LPPContext(DbContextOptions<LPPContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(this.Users));

            modelBuilder.Entity<User>().HasIndex(u => u.TelegramId).IsUnique();
        }
    }
}
