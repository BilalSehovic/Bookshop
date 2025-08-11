using WpfApp.Services;
using WpfApp.Tests.TestHelpers;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels
{
    public class SalesViewModelTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly SalesViewModel _viewModel;
        private readonly List<Book> _testBooks;

        public SalesViewModelTests()
        {
            _mockBookService = new Mock<IBookService>();
            _testBooks = TestData.CreateTestBooks();

            // Add out of stock book for testing
            _testBooks.Add(TestData.CreateTestBook("Out of Stock Book", stockQuantity: 0));

            // Setup default behavior
            _mockBookService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(_testBooks);

            _viewModel = new SalesViewModel(_mockBookService.Object);

            // Wait for initial load
            Task.Delay(100).Wait();
        }

        [Fact]
        public void Constructor_ShouldLoadBooks()
        {
            // Assert
            _viewModel.Books.Should().HaveCount(_testBooks.Count);
            _mockBookService.Verify(x => x.GetAllBooksAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public void Constructor_ShouldShowCorrectStatusMessage()
        {
            // Assert
            var inStockCount = _testBooks.Count(b => b.StockQuantity > 0);
            var outOfStockCount = _testBooks.Count(b => b.StockQuantity == 0);

            _viewModel
                .StatusText.Should()
                .Be(
                    $"{_testBooks.Count} books total ({inStockCount} in stock, {outOfStockCount} out of stock)."
                );
        }

        [Fact]
        public void SelectedBook_WhenInStockBookSelected_ShouldEnableSaleButton()
        {
            // Arrange
            var inStockBook = _testBooks.First(b => b.StockQuantity > 0);

            // Act
            _viewModel.SelectedBook = inStockBook;

            // Assert
            _viewModel
                .SelectedBookText.Should()
                .Be($"{inStockBook.Title} by {inStockBook.Author}");
            _viewModel.SalePriceText.Should().Be(inStockBook.Price.ToString("F2"));
            _viewModel.QuantityText.Should().Be("1");
            _viewModel.IsSellButtonEnabled.Should().BeTrue();
            _viewModel
                .StatusText.Should()
                .Be($"Ready to sell: {inStockBook.Title} (Stock: {inStockBook.StockQuantity})");
        }

        [Fact]
        public void SelectedBook_WhenOutOfStockBookSelected_ShouldDisableSaleButton()
        {
            // Arrange
            var outOfStockBook = _testBooks.First(b => b.StockQuantity == 0);

            // Act
            _viewModel.SelectedBook = outOfStockBook;

            // Assert
            _viewModel
                .SelectedBookText.Should()
                .Be($"{outOfStockBook.Title} by {outOfStockBook.Author}");
            _viewModel.SalePriceText.Should().Be(outOfStockBook.Price.ToString("F2"));
            _viewModel.QuantityText.Should().Be("1");
            _viewModel.IsSellButtonEnabled.Should().BeFalse();
            _viewModel.StatusText.Should().Be($"Selected: {outOfStockBook.Title} (OUT OF STOCK)");
        }

        [Fact]
        public void SelectedBook_WhenSetToNull_ShouldClearSelection()
        {
            // Arrange
            _viewModel.SelectedBook = _testBooks.First();

            // Act
            _viewModel.SelectedBook = null;

            // Assert
            _viewModel.SelectedBookText.Should().Be("No book selected");
            _viewModel.SalePriceText.Should().BeEmpty();
            _viewModel.QuantityText.Should().Be("1");
            _viewModel.IsSellButtonEnabled.Should().BeFalse();
            _viewModel.StatusText.Should().Be("Select a book to sell.");
        }

        [Fact]
        public void SellBookCommand_CanExecute_ShouldReturnCorrectValue()
        {
            // Arrange & Act & Assert
            _viewModel.SelectedBook = null;
            _viewModel.SellBookCommand.CanExecute(null).Should().BeFalse();

            var inStockBook = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = inStockBook;
            _viewModel.SellBookCommand.CanExecute(null).Should().BeTrue();

            var outOfStockBook = _testBooks.First(b => b.StockQuantity == 0);
            _viewModel.SelectedBook = outOfStockBook;
            _viewModel.SellBookCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public async Task SellBookCommand_WithValidData_ShouldProcessSale()
        {
            // Arrange
            var book = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = book;
            _viewModel.SalePriceText = "15.99";
            _viewModel.QuantityText = "2";

            _mockBookService.Setup(x => x.SellBookAsync(book.Id, 15.99, 2)).ReturnsAsync(true);

            // Act
            _viewModel.SellBookCommand.Execute(null);
            await Task.Delay(100); // Wait for async operation

            // Assert
            _mockBookService.Verify(x => x.SellBookAsync(book.Id, 15.99, 2), Times.Once);
            _mockBookService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(2)); // Initial load + refresh
        }

        [Fact]
        public void SellBookCommand_WithNoSelectedBook_ShouldNotCallService()
        {
            // Arrange
            _viewModel.SelectedBook = null;
            _viewModel.SalePriceText = "15.99";
            _viewModel.QuantityText = "1";

            // Act
            _viewModel.SellBookCommand.Execute(null);

            // Assert
            _mockBookService.Verify(
                x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()),
                Times.Never
            );
        }

        [Theory]
        [InlineData("invalid", "1")] // Invalid price
        [InlineData("0", "1")] // Zero price
        [InlineData("-5", "1")] // Negative price
        [InlineData("15.99", "invalid")] // Invalid quantity
        [InlineData("15.99", "0")] // Zero quantity
        [InlineData("15.99", "-1")] // Negative quantity
        public void SellBookCommand_WithInvalidInput_ShouldNotCallService(
            string price,
            string quantity
        )
        {
            // Arrange
            var book = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = book;
            _viewModel.SalePriceText = price;
            _viewModel.QuantityText = quantity;

            // Act
            _viewModel.SellBookCommand.Execute(null);

            // Assert
            _mockBookService.Verify(
                x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public void SellBookCommand_WithQuantityExceedingStock_ShouldNotCallService()
        {
            // Arrange
            var book = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = book;
            _viewModel.SalePriceText = "15.99";
            _viewModel.QuantityText = (book.StockQuantity + 1).ToString(); // Exceed stock

            // Act
            _viewModel.SellBookCommand.Execute(null);

            // Assert
            _mockBookService.Verify(
                x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()),
                Times.Never
            );
        }

        [Fact]
        public async Task RefreshView_ShouldReloadBooksAndUpdateStatus()
        {
            // Arrange
            var newBooks = TestData.CreateTestBooks(5);
            _mockBookService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(newBooks);

            // Act
            _viewModel.RefreshView();
            await Task.Delay(100);

            // Assert
            _viewModel.Books.Should().HaveCount(newBooks.Count);
            _viewModel.StatusText.Should().Contain($"{newBooks.Count} books total");
            _mockBookService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(2));
        }

        [Fact]
        public void PropertyChangedEvents_ShouldBeRaisedCorrectly()
        {
            // Arrange
            var propertyChangedEvents = new List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != null)
                    propertyChangedEvents.Add(args.PropertyName);
            };

            // Act
            _viewModel.SelectedBookText = "New Text";
            _viewModel.SalePriceText = "19.99";
            _viewModel.QuantityText = "2";
            _viewModel.IsSellButtonEnabled = true;
            _viewModel.StatusText = "New Status";

            // Assert
            propertyChangedEvents.Should().Contain(nameof(_viewModel.SelectedBookText));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.SalePriceText));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.QuantityText));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.IsSellButtonEnabled));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.StatusText));
        }

        [Fact]
        public async Task LoadBooksAsync_WhenServiceThrows_ShouldHandleGracefully()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            mockService
                .Setup(x => x.GetAllBooksAsync())
                .ThrowsAsync(new Exception("Service error"));

            var viewModel = new SalesViewModel(mockService.Object);
            await Task.Delay(100);

            // Assert
            viewModel.Books.Should().BeEmpty();
            viewModel.StatusText.Should().Be("Error loading books.");
        }

        [Fact]
        public async Task SellBookAsync_WhenServiceReturnsFalse_ShouldHandleFailure()
        {
            // Arrange
            var book = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = book;
            _viewModel.SalePriceText = "15.99";
            _viewModel.QuantityText = "1";

            _mockBookService
                .Setup(x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            _viewModel.SellBookCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            // Should refresh books after failed sale
            _mockBookService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(2));
        }

        [Fact]
        public async Task SellBookAsync_WhenServiceThrows_ShouldHandleException()
        {
            // Arrange
            var book = _testBooks.First(b => b.StockQuantity > 0);
            _viewModel.SelectedBook = book;
            _viewModel.SalePriceText = "15.99";
            _viewModel.QuantityText = "1";

            _mockBookService
                .Setup(x => x.SellBookAsync(It.IsAny<Guid>(), It.IsAny<double>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Sale failed"));

            // Act
            _viewModel.SellBookCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            _viewModel.StatusText.Should().Be("Sale failed.");
            _viewModel.IsSellButtonEnabled.Should().BeTrue(); // Should re-enable after error
        }
    }
}
