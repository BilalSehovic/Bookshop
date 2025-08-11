using WpfApp.Services;
using WpfApp.Tests.TestHelpers;
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

        //[Fact]
        //public async Task SalesReportViewModel_LoadAndExportWorkflow_ShouldWorkTogether()
        //{
        //    // Arrange
        //    var mockService = new Mock<IBookService>();
        //    var sales = TestData.CreateTestSales(5);
        //    var testDate = DateTime.Today;

        //    mockService.Setup(x => x.GetSalesByDateAsync(testDate)).ReturnsAsync(sales);

        //    var viewModel = new SalesReportViewModel(mockService.Object);
        //    await Task.Delay(200);

        //    // Act - Change date and verify data loads
        //    viewModel.SelectedDate = testDate.AddDays(-1);
        //    await Task.Delay(200);

        //    // Act - Use Today command
        //    viewModel.TodayCommand.Execute(null);
        //    await Task.Delay(200);

        //    // Assert
        //    viewModel.Sales.Should().HaveCount(sales.Count);
        //    viewModel.TotalSales.Should().Be(sales.Count);
        //    viewModel.BooksSold.Should().Be(sales.Sum(s => s.Quantity));
        //    viewModel.TotalRevenue.Should().Be(sales.Sum(s => s.UnitPrice * s.Quantity));
        //    viewModel.IsExportEnabled.Should().BeTrue();

        //    mockService.Verify(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()), Times.AtLeast(2));
        //}

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

        [Fact]
        public async Task ErrorHandling_ServiceExceptions_ShouldBeHandledGracefully()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            mockService
                .Setup(x => x.GetAllBooksAsync())
                .ThrowsAsync(new Exception("Database connection error"));

            // Act & Assert - BookManagementViewModel
            var bookManagementVM = new BookManagementViewModel(mockService.Object);
            await Task.Delay(100);

            bookManagementVM.Books.Should().BeEmpty();

            // Act & Assert - SalesViewModel
            var salesVM = new SalesViewModel(mockService.Object);
            await Task.Delay(100);

            salesVM.Books.Should().BeEmpty();
            salesVM.StatusText.Should().Be("Error loading books.");

            // Act & Assert - SalesReportViewModel
            mockService
                .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Report generation error"));

            var salesReportVM = new SalesReportViewModel(mockService.Object);
            await Task.Delay(200);

            salesReportVM.Sales.Should().BeEmpty();
            salesReportVM.IsExportEnabled.Should().BeFalse();
        }

        [Fact]
        public async Task CommandPatterns_CanExecuteStates_ShouldWorkCorrectly()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var books = TestData.CreateTestBooks(1).ToList();
            books[0].StockQuantity = 0; // Out of stock

            mockService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);

            var salesVM = new SalesViewModel(mockService.Object);
            await Task.Delay(100);

            // Act & Assert - Out of stock book should disable sell command
            salesVM.SelectedBook = books[0];
            salesVM.SellBookCommand.CanExecute(null).Should().BeFalse();
            salesVM.IsSellButtonEnabled.Should().BeFalse();

            // Simulate restocking
            books[0].StockQuantity = 5;
            salesVM.SelectedBook = books[0]; // Re-select to update state

            salesVM.SellBookCommand.CanExecute(null).Should().BeTrue();
            salesVM.IsSellButtonEnabled.Should().BeTrue();
        }

        [Fact]
        public async Task ViewModelLifecycle_InitializationAndCleanup_ShouldWorkCorrectly()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var books = TestData.CreateTestBooks();

            mockService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);

            // Act - Create and initialize ViewModels
            var bookManagementVM = new BookManagementViewModel(mockService.Object);
            var salesVM = new SalesViewModel(mockService.Object);

            await Task.Delay(100);

            // Assert - ViewModels should be properly initialized
            bookManagementVM.Books.Should().HaveCount(books.Count);
            salesVM.Books.Should().HaveCount(books.Count);

            // Commands should be available
            bookManagementVM.AddCommand.Should().NotBeNull();
            bookManagementVM.UpdateCommand.Should().NotBeNull();
            bookManagementVM.DeleteCommand.Should().NotBeNull();
            bookManagementVM.ClearCommand.Should().NotBeNull();

            salesVM.SellBookCommand.Should().NotBeNull();

            // Initial states should be correct
            bookManagementVM.IsUpdateEnabled.Should().BeFalse();
            bookManagementVM.IsDeleteEnabled.Should().BeFalse();
            salesVM.IsSellButtonEnabled.Should().BeFalse();
            salesVM.SelectedBookText.Should().Be("No book selected");
        }
    }
}
