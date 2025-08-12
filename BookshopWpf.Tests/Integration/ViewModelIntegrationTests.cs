using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Tests.Integration
{
    /// <summary>
    /// Integration tests that verify ViewModels work correctly with Services
    /// </summary>
    public class ViewModelIntegrationTests
    {
        [Fact]
        public async Task BookManagementViewModel_AddUpdateDelete_ShouldWorkTogether()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var books = TestData.CreateTestBooks(2).ToList();
            var newBook = TestData.CreateTestBook("New Book");

            mockService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);
            mockService
                .Setup(x => x.AddBookAsync(It.IsAny<Book>()))
                .ReturnsAsync(newBook)
                .Callback<Book>(b => books.Add(b));
            mockService.Setup(x => x.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(newBook);
            mockService
                .Setup(x => x.DeleteBookAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true)
                .Callback<Guid>(id => books.RemoveAll(b => b.Id == id));

            var viewModel = new BookManagementViewModel(mockService.Object);
            await Task.Delay(100); // Wait for initial load

            // Test Add functionality
            viewModel.Title = newBook.Title;
            viewModel.Author = newBook.Author;
            viewModel.Isbn = newBook.Isbn;
            viewModel.Price = newBook.Price.ToString("F2");
            viewModel.Stock = newBook.StockQuantity.ToString();

            viewModel.AddCommand.Execute(null);
            await Task.Delay(100);

            // Test Update functionality
            var bookToUpdate = viewModel.Books.First();
            viewModel.SelectedBook = bookToUpdate;
            viewModel.Title = "Updated Title";

            viewModel.UpdateCommand.Execute(null);
            await Task.Delay(100);

            // Test Delete functionality
            var bookToDelete = viewModel.Books.First();
            viewModel.SelectedBook = bookToDelete;

            viewModel.DeleteCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            mockService.Verify(x => x.AddBookAsync(It.IsAny<Book>()), Times.Once);
            mockService.Verify(x => x.UpdateBookAsync(It.IsAny<Book>()), Times.Once);
            mockService.Verify(x => x.DeleteBookAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task SalesViewModel_LoadBooksAndSell_ShouldWorkTogether()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var books = TestData.CreateTestBooks(3).ToList();
            var bookToSell = books.First();

            mockService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);
            mockService
                .Setup(x => x.SellBookAsync(bookToSell.Id, It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var viewModel = new SalesViewModel(mockService.Object);
            await Task.Delay(100);

            // Act - Select book and sell
            viewModel.SelectedBook = bookToSell;
            viewModel.SalePriceText = "15.99";
            viewModel.QuantityText = "2";

            viewModel.SellBookCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            viewModel.Books.Should().Contain(bookToSell);
            mockService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(2)); // Initial + refresh after sale
            mockService.Verify(x => x.SellBookAsync(bookToSell.Id, 15.99, 2), Times.Once);
        }

        [Fact]
        public async Task SalesReportViewModel_LoadAndExportWorkflow_ShouldWorkTogether()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var initialBooks = TestData.CreateTestBooks(3).ToList();
            var sales = TestData.CreateTestSales(initialBooks, 5);
            var testDate = DateTime.Today;

            mockService.Setup(x => x.GetSalesByDateAsync(testDate)).ReturnsAsync(sales);

            var viewModel = new SalesReportViewModel(mockService.Object);
            await Task.Delay(200);

            // Act - Change date and verify data loads
            viewModel.SelectedDate = testDate.AddDays(-1);
            await Task.Delay(200);

            // Act - Use Today command
            viewModel.TodayCommand.Execute(null);
            await Task.Delay(200);

            // Assert
            viewModel.Sales.Should().HaveCount(sales.Count);
            viewModel.TotalSales.Should().Be(sales.Count);
            viewModel.BooksSold.Should().Be(sales.Sum(s => s.Quantity));
            viewModel.TotalRevenue.Should().Be(sales.Sum(s => s.UnitPrice * s.Quantity));
            viewModel.IsExportEnabled.Should().BeTrue();

            mockService.Verify(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()), Times.AtLeast(2));
        }

        [Fact]
        public async Task CrossViewModel_DataConsistency_ShouldMaintainState()
        {
            // Arrange - Shared service instance
            var mockService = new Mock<IBookService>();
            var initialBooks = TestData.CreateTestBooks(3).ToList();
            var sales = TestData.CreateTestSales(initialBooks);

            mockService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(initialBooks);
            mockService.Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(sales);
            mockService
                .Setup(x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Create multiple ViewModels using the same service
            var bookManagementVM = new BookManagementViewModel(mockService.Object);
            var salesVM = new SalesViewModel(mockService.Object);
            var salesReportVM = new SalesReportViewModel(mockService.Object);

            await Task.Delay(200); // Wait for initial loads

            // Act - Make a sale through SalesViewModel
            var bookToSell = salesVM.Books.First();
            salesVM.SelectedBook = bookToSell;
            salesVM.SalePriceText = "19.99";
            salesVM.QuantityText = "1";

            salesVM.SellBookCommand.Execute(null);
            await Task.Delay(100);

            // Act - Refresh other ViewModels
            bookManagementVM.RefreshView();
            salesReportVM.RefreshReport();
            await Task.Delay(200);

            // Assert - All ViewModels should reflect the updated state
            mockService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(4)); // Multiple refreshes
            mockService.Verify(x => x.SellBookAsync(It.IsAny<Guid>(), 19.99, 1), Times.Once);
            mockService.Verify(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()), Times.AtLeast(2));

            // All ViewModels should be working with consistent data
            bookManagementVM.Books.Should().NotBeEmpty();
            salesVM.Books.Should().NotBeEmpty();
            salesReportVM.Sales.Should().NotBeEmpty();
        }
    }
}
