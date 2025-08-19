using System.Windows;
using WpfApp.Interfaces;
using WpfApp.Services;

namespace BookshopWpf.Tests.Helpers;

public static class ServicesMoqHelper
{
    public static Mock<IBookService> GetBookServiceMock()
    {
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

        return mockService;
    }

    public static Mock<IDialogService> GetDialogServiceMock()
    {
        var mockDialogService = new Mock<IDialogService>();

        mockDialogService
            .Setup(x =>
                x.ShowInformation(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButton>(),
                    It.IsAny<MessageBoxImage>()
                )
            )
            .Verifiable();

        mockDialogService
            .Setup(x =>
                x.ShowWarning(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButton>(),
                    It.IsAny<MessageBoxImage>()
                )
            )
            .Verifiable();
        mockDialogService
            .Setup(x =>
                x.ShowError(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButton>(),
                    It.IsAny<MessageBoxImage>()
                )
            )
            .Verifiable();
        mockDialogService
            .Setup(x =>
                x.ShowConfirmation(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButton>(),
                    It.IsAny<MessageBoxImage>()
                )
            )
            .Returns(true);

        return mockDialogService;
    }
}
