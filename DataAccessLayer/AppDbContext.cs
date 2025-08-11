using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn).IsUnique();
                entity.Property(e => e.Price).HasPrecision(10, 2);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.UnitPrice).HasPrecision(10, 2);
                entity
                    .HasOne(s => s.Book)
                    .WithMany()
                    .HasForeignKey(s => s.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
