using AdvisorManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdvisorManagement.Infrastructure
{
    public class AdvisorContext : DbContext
    {
        public AdvisorContext(DbContextOptions<AdvisorContext> options) : base(options) { }

        public DbSet<Advisor> Advisors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advisor>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Advisor>()
                .Property(a => a.SIN)
                .IsRequired()
                .HasMaxLength(9);

            modelBuilder.Entity<Advisor>()
                .HasIndex(a => a.SIN)
                .IsUnique();

            modelBuilder.Entity<Advisor>()
                .Property(a => a.Address)
                .HasMaxLength(255);

            modelBuilder.Entity<Advisor>()
                .Property(a => a.Phone)
                .HasMaxLength(8);
        }
    }
}
