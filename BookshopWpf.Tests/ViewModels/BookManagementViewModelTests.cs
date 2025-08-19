using BookshopWpf.Tests.Helpers;
using WpfApp.Interfaces;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels;

public class BookManagementViewModelTests
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<IDialogService> _mockDialogService;
    private readonly BookManagementViewModel _viewModel;
    private readonly List<Book> _testBooks;

    public BookManagementViewModelTests()
    {
        _mockBookService = ServicesMoqHelper.GetBookServiceMock();
        _mockDialogService = ServicesMoqHelper.GetDialogServiceMock();
        _testBooks = TestData.CreateTestBooks();

        _mockBookService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(_testBooks);

        _viewModel = new BookManagementViewModel(
            _mockBookService.Object,
            _mockDialogService.Object
        );

        // Wait for initial load to complete
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
    public void SelectedBook_WhenSet_ShouldPopulateForm()
    {
        // Arrange
        var book = _testBooks.First();

        // Act
        _viewModel.SelectedBook = book;

        // Assert
        _viewModel.Title.Should().Be(book.Title);
        _viewModel.Author.Should().Be(book.Author);
        _viewModel.Isbn.Should().Be(book.Isbn);
        _viewModel.Price.Should().Be(book.Price.ToString("F2"));
        _viewModel.Stock.Should().Be(book.StockQuantity.ToString());
        _viewModel.IsUpdateEnabled.Should().BeTrue();
        _viewModel.IsDeleteEnabled.Should().BeTrue();
    }

    [Fact]
    public void ClearCommand_ShouldClearAllFields()
    {
        // Arrange
        _viewModel.SelectedBook = _testBooks.First();
        _viewModel.Title = "Test";
        _viewModel.Author = "Test";

        // Act
        _viewModel.ClearCommand.Execute(null);

        // Assert
        _viewModel.Title.Should().BeEmpty();
        _viewModel.Author.Should().BeEmpty();
        _viewModel.Isbn.Should().BeEmpty();
        _viewModel.Price.Should().BeEmpty();
        _viewModel.Stock.Should().BeEmpty();
        _viewModel.SelectedBook.Should().BeNull();
        _viewModel.IsUpdateEnabled.Should().BeFalse();
        _viewModel.IsDeleteEnabled.Should().BeFalse();
    }

    [Fact]
    public void AddCommand_WithInvalidData_ShouldNotCallService()
    {
        // Arrange - Empty title (invalid)
        _viewModel.Title = "";
        _viewModel.Author = "Test Author";
        _viewModel.Isbn = "123456789";
        _viewModel.Price = "19.99";
        _viewModel.Stock = "10";

        // Act
        _viewModel.AddCommand.Execute(null);

        // Assert
        _mockBookService.Verify(x => x.AddBookAsync(It.IsAny<Book>()), Times.Never);
    }

    [Theory]
    [InlineData("", "Author", "ISBN", "19.99", "10")] // Empty title
    [InlineData("Title", "", "ISBN", "19.99", "10")] // Empty author
    [InlineData("Title", "Author", "", "19.99", "10")] // Empty ISBN
    [InlineData("Title", "Author", "ISBN", "invalid", "10")] // Invalid price
    [InlineData("Title", "Author", "ISBN", "-1", "10")] // Negative price
    [InlineData("Title", "Author", "ISBN", "19.99", "invalid")] // Invalid stock
    [InlineData("Title", "Author", "ISBN", "19.99", "-1")] // Negative stock
    public void ValidationScenarios_ShouldPreventInvalidOperations(
        string title,
        string author,
        string isbn,
        string price,
        string stock
    )
    {
        // Arrange
        _viewModel.Title = title;
        _viewModel.Author = author;
        _viewModel.Isbn = isbn;
        _viewModel.Price = price;
        _viewModel.Stock = stock;

        // Act
        _viewModel.AddCommand.Execute(null);

        // Assert
        _mockBookService.Verify(x => x.AddBookAsync(It.IsAny<Book>()), Times.Never);
    }
}
