namespace WpfApp.Tests.Helpers
{
    /// <summary>
    /// Provides test data for unit tests
    /// </summary>
    public static class TestData
    {
        public static Book CreateTestBook(
            string title = "Test Book",
            string author = "Test Author",
            string isbn = "978-0123456789",
            double price = 19.99,
            int stockQuantity = 10
        )
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Author = author,
                Isbn = isbn,
                Price = price,
                StockQuantity = stockQuantity,
                CreatedAt = DateTime.UtcNow,
            };
        }

        public static List<Book> CreateTestBooks(int count = 3)
        {
            var books = new List<Book>();
            for (int i = 1; i <= count; i++)
            {
                books.Add(
                    CreateTestBook(
                        title: $"Test Book {i}",
                        author: $"Test Author {i}",
                        isbn: $"978-012345678{i}",
                        price: 10.00 + i,
                        stockQuantity: i * 5
                    )
                );
            }
            return books;
        }

        public static Sale CreateTestSale(
            Book? book = null,
            double unitPrice = 19.99,
            int quantity = 1
        )
        {
            return new Sale
            {
                Id = Guid.NewGuid(),
                BookId = book?.Id ?? Guid.NewGuid(),
                Book = book,
                UnitPrice = unitPrice,
                Quantity = quantity,
                SaleDate = DateTime.UtcNow,
            };
        }

        public static List<Sale> CreateTestSales(List<Book>? books = null, int count = 3)
        {
            books ??= CreateTestBooks(count);
            var sales = new List<Sale>();

            for (int i = 0; i < count; i++)
            {
                var book = i < books.Count ? books[i] : CreateTestBook();
                sales.Add(CreateTestSale(book, 15.00 + i, i + 1));
            }

            return sales;
        }
    }
}
