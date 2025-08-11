using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public static class AppDbSeeded
{
    public static void SeedBooks(this ModelBuilder modelBuilder)
    {
        // Seed books
        modelBuilder
            .Entity<Book>()
            .HasData(
                new Book
                {
                    Id = Guid.Parse("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"),
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0547928227",
                    Price = 9.99,
                    StockQuantity = 1,
                },
                new Book
                {
                    Id = Guid.Parse("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"),
                    Title = "1984",
                    Author = "George Orwell",
                    Isbn = "978-0451524935",
                    Price = 7.99,
                    StockQuantity = 28,
                },
                new Book
                {
                    Id = Guid.Parse("b31f1f26-1420-4c54-a25f-d344bb8a7e23"),
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = "978-0061120084",
                    Price = 8.49,
                    StockQuantity = 24,
                },
                new Book
                {
                    Id = Guid.Parse("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"),
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Isbn = "978-1503290563",
                    Price = 6.99,
                    StockQuantity = 31,
                },
                new Book
                {
                    Id = Guid.Parse("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"),
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Isbn = "978-0743273565",
                    Price = 7.49,
                    StockQuantity = 13,
                },
                new Book
                {
                    Id = Guid.Parse("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"),
                    Title = "Moby-Dick",
                    Author = "Herman Melville",
                    Isbn = "978-1503280786",
                    Price = 10.99,
                    StockQuantity = 12,
                },
                new Book
                {
                    Id = Guid.Parse("c7d0f9c6-9c71-4370-bf54-93b47c987edf"),
                    Title = "War and Peace",
                    Author = "Leo Tolstoy",
                    Isbn = "978-1400079988",
                    Price = 12.99,
                    StockQuantity = 19,
                },
                new Book
                {
                    Id = Guid.Parse("6b1894a2-71cb-4a85-815b-c245b53c7923"),
                    Title = "Crime and Punishment",
                    Author = "Fyodor Dostoevsky",
                    Isbn = "978-0486415871",
                    Price = 11.49,
                    StockQuantity = 50,
                },
                new Book
                {
                    Id = Guid.Parse("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"),
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    Isbn = "978-0316769488",
                    Price = 8.99,
                    StockQuantity = 16,
                },
                new Book
                {
                    Id = Guid.Parse("4453c2c5-f2df-46c0-b3d4-482f7b37602e"),
                    Title = "Brave New World",
                    Author = "Aldous Huxley",
                    Isbn = "978-0060850524",
                    Price = 7.89,
                    StockQuantity = 46,
                }
            );
    }
}
