using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp.Tests.ViewModels;

public class MainWindowViewModelTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<DbSet<Book>> _mockDbSet;
    private readonly MainWindowViewModel _viewModel;
    private readonly List<Book> _testBooks;

    public MainWindowViewModelTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockDbContext = new Mock<AppDbContext>();
        _mockDbSet = new Mock<DbSet<Book>>();
        _testBooks = TestData.CreateTestBooks();

        // Setup mock views
        var mockBookManagementView = new Mock<BookManagementView>(
            Mock.Of<BookManagementViewModel>()
        );
        var mockSalesView = new Mock<SalesView>(Mock.Of<SalesViewModel>());
        var mockSalesReportView = new Mock<SalesReportView>(Mock.Of<SalesReportViewModel>());

        // Setup service provider
        _mockServiceProvider
            .Setup(x => x.GetRequiredService<BookManagementView>())
            .Returns(mockBookManagementView.Object);
        _mockServiceProvider
            .Setup(x => x.GetRequiredService<SalesView>())
            .Returns(mockSalesView.Object);
        _mockServiceProvider
            .Setup(x => x.GetRequiredService<SalesReportView>())
            .Returns(mockSalesReportView.Object);
        _mockServiceProvider
            .Setup(x => x.GetRequiredService<AppDbContext>())
            .Returns(_mockDbContext.Object);

        // Setup DbContext mock
        _mockDbSet
            .As<IQueryable<Book>>()
            .Setup(m => m.Provider)
            .Returns(_testBooks.AsQueryable().Provider);
        _mockDbSet
            .As<IQueryable<Book>>()
            .Setup(m => m.Expression)
            .Returns(_testBooks.AsQueryable().Expression);
        _mockDbSet
            .As<IQueryable<Book>>()
            .Setup(m => m.ElementType)
            .Returns(_testBooks.AsQueryable().ElementType);
        _mockDbSet
            .As<IQueryable<Book>>()
            .Setup(m => m.GetEnumerator())
            .Returns(_testBooks.AsQueryable().GetEnumerator());

        _mockDbContext.Setup(x => x.Books).Returns(_mockDbSet.Object);
        _mockDbContext.Setup(x => x.Database.EnsureCreatedAsync(default)).ReturnsAsync(true);
        _mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        _viewModel = new MainWindowViewModel(_mockServiceProvider.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeWithBookManagementView()
    {
        // Assert
        _viewModel.CurrentContent.Should().NotBeNull();
        _viewModel.CurrentContent.Should().BeOfType<BookManagementView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Book Management");
    }

    [Fact]
    public void LoadSalesCommand_ShouldLoadSalesView()
    {
        // Act
        _viewModel.LoadSalesCommand.Execute(null);

        // Assert
        _viewModel.CurrentContent.Should().NotBeNull();
        _viewModel.CurrentContent.Should().BeOfType<SalesView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Sales");
    }

    [Fact]
    public void LoadSalesReportCommand_ShouldLoadSalesReportView()
    {
        // Act
        _viewModel.LoadSalesReportCommand.Execute(null);

        // Assert
        _viewModel.CurrentContent.Should().NotBeNull();
        _viewModel.CurrentContent.Should().BeOfType<SalesReportView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Sales Report");
    }
}
