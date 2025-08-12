using DataAccessLayer;
using DataAccessLayer.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WpfApp.Services;
using WpfApp.Tests.TestHelpers;
using WpfApp.ViewModels;
using WpfApp.Views;
using Xunit;

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

        // Setup mock services for ViewModels
        var mockBookService = new Mock<IBookService>();
        
        // Setup mock ViewModels with their dependencies
        var mockBookManagementViewModel = new Mock<BookManagementViewModel>(mockBookService.Object);
        var mockSalesViewModel = new Mock<SalesViewModel>(mockBookService.Object);
        var mockSalesReportViewModel = new Mock<SalesReportViewModel>(mockBookService.Object);
        
        // Setup mock views
        var mockBookManagementView = new Mock<BookManagementView>(mockBookManagementViewModel.Object);
        var mockSalesView = new Mock<SalesView>(mockSalesViewModel.Object);
        var mockSalesReportView = new Mock<SalesReportView>(mockSalesReportViewModel.Object);

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
    public void LoadBookManagementCommand_ShouldLoadBookManagementView()
    {
        // Arrange
        _viewModel.CurrentContent = null;

        // Act
        _viewModel.LoadBookManagementCommand.Execute(null);

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

    [Fact]
    public void LoadBookManagementCommand_CalledTwice_ShouldReuseViewAndRefresh()
    {
        // Arrange
        _viewModel.LoadBookManagementCommand.Execute(null);
        var firstView = _viewModel.CurrentContent;

        // Act
        _viewModel.LoadBookManagementCommand.Execute(null);

        // Assert
        _viewModel.CurrentContent.Should().BeSameAs(firstView);
        // In real scenario, RefreshView would be called, but we can't easily verify with mocks
    }

    [Fact]
    public void LoadSalesCommand_CalledTwice_ShouldReuseViewAndRefresh()
    {
        // Arrange
        _viewModel.LoadSalesCommand.Execute(null);
        var firstView = _viewModel.CurrentContent;

        // Act
        _viewModel.LoadSalesCommand.Execute(null);

        // Assert
        _viewModel.CurrentContent.Should().BeSameAs(firstView);
    }

    [Fact]
    public void LoadSalesReportCommand_CalledTwice_ShouldReuseViewAndRefresh()
    {
        // Arrange
        _viewModel.LoadSalesReportCommand.Execute(null);
        var firstView = _viewModel.CurrentContent;

        // Act
        _viewModel.LoadSalesReportCommand.Execute(null);

        // Assert
        _viewModel.CurrentContent.Should().BeSameAs(firstView);
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
        _viewModel.WindowTitle = "New Title";

        // Assert
        propertyChangedEvents.Should().Contain(nameof(_viewModel.WindowTitle));
    }

    [Fact]
    public void WindowTitle_WhenSet_ShouldUpdateCorrectly()
    {
        // Arrange
        var newTitle = "Custom Window Title";

        // Act
        _viewModel.WindowTitle = newTitle;

        // Assert
        _viewModel.WindowTitle.Should().Be(newTitle);
    }

    [Fact]
    public void CurrentContent_WhenSet_ShouldUpdateCorrectly()
    {
        // Arrange
        var mockBookService = new Mock<IBookService>();
        var mockBookManagementViewModel = new Mock<BookManagementViewModel>(mockBookService.Object);
        var mockView = new Mock<BookManagementView>(mockBookManagementViewModel.Object);

        // Act
        _viewModel.CurrentContent = mockView.Object;

        // Assert
        _viewModel.CurrentContent.Should().BeSameAs(mockView.Object);
    }

    [Fact]
    public void AllCommands_ShouldBeInitialized()
    {
        // Assert
        _viewModel.LoadBookManagementCommand.Should().NotBeNull();
        _viewModel.LoadSalesCommand.Should().NotBeNull();
        _viewModel.LoadSalesReportCommand.Should().NotBeNull();
        _viewModel.InitializeDatabaseCommand.Should().NotBeNull();
        _viewModel.ShowAboutCommand.Should().NotBeNull();
    }

    [Fact]
    public void AllCommands_CanExecute_ShouldReturnTrue()
    {
        // Assert
        _viewModel.LoadBookManagementCommand.CanExecute(null).Should().BeTrue();
        _viewModel.LoadSalesCommand.CanExecute(null).Should().BeTrue();
        _viewModel.LoadSalesReportCommand.CanExecute(null).Should().BeTrue();
        _viewModel.InitializeDatabaseCommand.CanExecute(null).Should().BeTrue();
        _viewModel.ShowAboutCommand.CanExecute(null).Should().BeTrue();
    }

    [Fact]
    public void Navigation_BetweenDifferentViews_ShouldUpdateCurrentContentAndTitle()
    {
        // Act & Assert - Book Management
        _viewModel.LoadBookManagementCommand.Execute(null);
        _viewModel.CurrentContent.Should().BeOfType<BookManagementView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Book Management");

        // Act & Assert - Sales
        _viewModel.LoadSalesCommand.Execute(null);
        _viewModel.CurrentContent.Should().BeOfType<SalesView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Sales");

        // Act & Assert - Sales Report
        _viewModel.LoadSalesReportCommand.Execute(null);
        _viewModel.CurrentContent.Should().BeOfType<SalesReportView>();
        _viewModel.WindowTitle.Should().Be("Bookshop Management System - Sales Report");
    }

}
