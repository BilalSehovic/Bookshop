using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels;

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
    public void SelectedBook_WhenInStockBookSelected_ShouldEnableSaleButton()
    {
        // Arrange
        var inStockBook = _testBooks.First(b => b.StockQuantity > 0);

        // Act
        _viewModel.SelectedBook = inStockBook;

        // Assert
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
        _viewModel.IsSellButtonEnabled.Should().BeFalse();
        _viewModel.StatusText.Should().Be($"Selected: {outOfStockBook.Title} (OUT OF STOCK)");
    }

    [Theory]
    [InlineData("invalid", "1")] // Invalid price
    [InlineData("0", "1")] // Zero price
    [InlineData("-5", "1")] // Negative price
    [InlineData("15.99", "invalid")] // Invalid quantity
    [InlineData("15.99", "0")] // Zero quantity
    [InlineData("15.99", "-1")] // Negative quantity
    public void SellBookCommand_WithInvalidInput_ShouldNotCallService(string price, string quantity)
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
}
