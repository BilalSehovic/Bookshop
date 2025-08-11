using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                    .Property(e => e.SaleDate)
                    .HasConversion(
                        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
                entity
                    .HasOne(s => s.Book)
                    .WithMany()
                    .HasForeignKey(s => s.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            var utcConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (
                    var property in entityType
                        .GetProperties()
                        .Where(p => p.ClrType == typeof(DateTime))
                )
                {
                    property.SetValueConverter(utcConverter);
                }
            }
        }
    }
}
