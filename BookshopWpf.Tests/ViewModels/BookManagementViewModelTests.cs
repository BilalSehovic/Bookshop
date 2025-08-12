using WpfApp.Services;
using WpfApp.Tests.TestHelpers;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels;

public class BookManagementViewModelTests
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly BookManagementViewModel _viewModel;
    private readonly List<Book> _testBooks;

    public BookManagementViewModelTests()
    {
        _mockBookService = new Mock<IBookService>();
        _testBooks = TestData.CreateTestBooks();

        _mockBookService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(_testBooks);

        _viewModel = new BookManagementViewModel(_mockBookService.Object);

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
    public void SelectedBook_WhenSetToNull_ShouldDisableButtons()
    {
        // Arrange
        _viewModel.SelectedBook = _testBooks.First();

        // Act
        _viewModel.SelectedBook = null;

        // Assert
        _viewModel.IsUpdateEnabled.Should().BeFalse();
        _viewModel.IsDeleteEnabled.Should().BeFalse();
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
    public async Task AddCommand_WithValidData_ShouldAddBookAndRefresh()
    {
        // Arrange
        var newBook = TestData.CreateTestBook("New Book", "New Author");
        _mockBookService.Setup(x => x.AddBookAsync(It.IsAny<Book>())).ReturnsAsync(newBook);
        _mockBookService
            .Setup(x => x.GetAllBooksAsync())
            .ReturnsAsync(_testBooks.Concat(new[] { newBook }).ToList());

        _viewModel.Title = newBook.Title;
        _viewModel.Author = newBook.Author;
        _viewModel.Isbn = newBook.Isbn;
        _viewModel.Price = newBook.Price.ToString("F2");
        _viewModel.Stock = newBook.StockQuantity.ToString();

        // Act
        _viewModel.AddCommand.Execute(null);
        await Task.Delay(100); // Wait for async operations

        // Assert
        _mockBookService.Verify(
            x =>
                x.AddBookAsync(
                    It.Is<Book>(b => b.Title == newBook.Title && b.Author == newBook.Author)
                ),
            Times.Once
        );
        _mockBookService.Verify(x => x.GetAllBooksAsync(), Times.AtLeast(2));
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

    [Fact]
    public async Task UpdateCommand_WithValidDataAndSelectedBook_ShouldUpdateBook()
    {
        // Arrange
        var bookToUpdate = _testBooks.First();
        _viewModel.SelectedBook = bookToUpdate;
        _viewModel.Title = "Updated Title";
        _viewModel.Author = "Updated Author";
        _viewModel.Isbn = "Updated ISBN";
        _viewModel.Price = "25.99";
        _viewModel.Stock = "15";

        _mockBookService.Setup(x => x.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(bookToUpdate);

        // Act
        _viewModel.UpdateCommand.Execute(null);
        await Task.Delay(100);

        // Assert
        _mockBookService.Verify(
            x =>
                x.UpdateBookAsync(
                    It.Is<Book>(b => b.Id == bookToUpdate.Id && b.Title == "Updated Title")
                ),
            Times.Once
        );
    }

    [Fact]
    public void UpdateCommand_WithNoSelectedBook_ShouldNotCallService()
    {
        // Arrange
        _viewModel.SelectedBook = null;
        _viewModel.Title = "Test";

        // Act
        _viewModel.UpdateCommand.Execute(null);

        // Assert
        _mockBookService.Verify(x => x.UpdateBookAsync(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public void UpdateCommand_CanExecute_ShouldReturnCorrectValue()
    {
        // Arrange & Act & Assert
        _viewModel.SelectedBook = null;
        _viewModel.UpdateCommand.CanExecute(null).Should().BeFalse();

        _viewModel.SelectedBook = _testBooks.First();
        _viewModel.UpdateCommand.CanExecute(null).Should().BeTrue();
    }

    [Fact]
    public void DeleteCommand_CanExecute_ShouldReturnCorrectValue()
    {
        // Arrange & Act & Assert
        _viewModel.SelectedBook = null;
        _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();

        _viewModel.SelectedBook = _testBooks.First();
        _viewModel.DeleteCommand.CanExecute(null).Should().BeTrue();
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

    [Fact]
    public async Task RefreshView_ShouldReloadBooks()
    {
        // Arrange
        var newBooks = TestData.CreateTestBooks(5);
        _mockBookService.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(newBooks);

        // Act
        _viewModel.RefreshView();
        await Task.Delay(100);

        // Assert
        _viewModel.Books.Should().HaveCount(newBooks.Count);
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
        _viewModel.Title = "New Title";
        _viewModel.Author = "New Author";
        _viewModel.IsUpdateEnabled = true;

        // Assert
        propertyChangedEvents.Should().Contain(nameof(_viewModel.Title));
        propertyChangedEvents.Should().Contain(nameof(_viewModel.Author));
        propertyChangedEvents.Should().Contain(nameof(_viewModel.IsUpdateEnabled));
    }
}
