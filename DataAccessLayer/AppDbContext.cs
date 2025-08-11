using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Person>()
                .Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Book>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Sale>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Sale>().HasOne(s => s.Book).WithMany().HasForeignKey(s => s.BookId);

            modelBuilder.SeedBooks();
        }
    }
}
