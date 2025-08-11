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
                    StockQuantity = 2,
                },
                new Book
                {
                    Id = Guid.Parse("b31f1f26-1420-4c54-a25f-d344bb8a7e23"),
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = "978-0061120084",
                    Price = 8.49,
                    StockQuantity = 15,
                },
                new Book
                {
                    Id = Guid.Parse("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"),
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Isbn = "978-1503290563",
                    Price = 6.99,
                    StockQuantity = 14,
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
                    StockQuantity = 20,
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
                    StockQuantity = 5,
                },
                new Book
                {
                    Id = Guid.Parse("4453c2c5-f2df-46c0-b3d4-482f7b37602e"),
                    Title = "Brave New World",
                    Author = "Aldous Huxley",
                    Isbn = "978-0060850524",
                    Price = 7.89,
                    StockQuantity = 100,
                },
                new Book
                {
                    Id = Guid.Parse("f6332d83-5ed3-41f2-bc4b-5f93c97a3e07"),
                    Title = "The Lord of the Rings",
                    Author = "J.R.R. Tolkien",
                    Isbn = "978-0618640157",
                    Price = 14.99,
                    StockQuantity = 70,
                },
                new Book
                {
                    Id = Guid.Parse("8c276e4b-b1a0-4e72-b15f-f8b9b2e6fd72"),
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Author = "J.K. Rowling",
                    Isbn = "978-0590353427",
                    Price = 9.49,
                    StockQuantity = 50,
                },
                new Book
                {
                    Id = Guid.Parse("20a6ec6e-d4d8-4ff1-8e19-3ff0c67dc8ec"),
                    Title = "The Chronicles of Narnia",
                    Author = "C.S. Lewis",
                    Isbn = "978-0066238500",
                    Price = 8.79,
                    StockQuantity = 30,
                },
                new Book
                {
                    Id = Guid.Parse("c771cf23-9a6d-4e77-8f8f-1f58692eb3a7"),
                    Title = "Anna Karenina",
                    Author = "Leo Tolstoy",
                    Isbn = "978-0143035008",
                    Price = 11.99,
                    StockQuantity = 30,
                },
                new Book
                {
                    Id = Guid.Parse("b0c85cb0-6041-40d4-a0d0-3df4d9e0c733"),
                    Title = "The Alchemist",
                    Author = "Paulo Coelho",
                    Isbn = "978-0061122415",
                    Price = 8.29,
                    StockQuantity = 14,
                },
                new Book
                {
                    Id = Guid.Parse("3f84bb7a-2eb5-4a21-bb16-74af4f16fbbf"),
                    Title = "Les Misérables",
                    Author = "Victor Hugo",
                    Isbn = "978-0451419439",
                    Price = 12.49,
                    StockQuantity = 10,
                },
                new Book
                {
                    Id = Guid.Parse("e78a8c29-9382-4d53-b5cb-8265f8b2a27f"),
                    Title = "Dracula",
                    Author = "Bram Stoker",
                    Isbn = "978-0486411095",
                    Price = 7.19,
                    StockQuantity = 70,
                },
                new Book
                {
                    Id = Guid.Parse("9df23a68-8b7e-43b3-a7f2-fb32cb0f89f3"),
                    Title = "Frankenstein",
                    Author = "Mary Shelley",
                    Isbn = "978-0486282114",
                    Price = 6.89,
                    StockQuantity = 50,
                },
                new Book
                {
                    Id = Guid.Parse("b0af9fd1-3a8e-4ef4-90aa-2f86b1c34f66"),
                    Title = "The Kite Runner",
                    Author = "Khaled Hosseini",
                    Isbn = "978-1594631931",
                    Price = 9.19,
                    StockQuantity = 30,
                },
                new Book
                {
                    Id = Guid.Parse("d6b2df6f-52d4-4098-9957-6cb8ce00c9a5"),
                    Title = "A Tale of Two Cities",
                    Author = "Charles Dickens",
                    Isbn = "978-1503219700",
                    Price = 8.59,
                    StockQuantity = 20,
                }
            );
    }
}
