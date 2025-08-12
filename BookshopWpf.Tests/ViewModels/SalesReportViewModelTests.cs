using System.IO;
using WpfApp.Services;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels;

public class SalesReportViewModelTests : IDisposable
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly SalesReportViewModel _viewModel;
    private readonly List<Sale> _testSales;
    private readonly List<string> _tempFiles;

    public SalesReportViewModelTests()
    {
        _mockBookService = new Mock<IBookService>();
        _testSales = TestData.CreateTestSales();
        _tempFiles = new List<string>();

        // Setup default behavior
        _mockBookService
            .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(_testSales);

        _viewModel = new SalesReportViewModel(_mockBookService.Object);

        // Wait for initial load
        Task.Delay(200).Wait();
    }

    public void Dispose()
    {
        // Cleanup temp files
        foreach (var file in _tempFiles.Where(File.Exists))
        {
            try
            {
                File.Delete(file);
            }
            catch { }
        }
    }

    [Fact]
    public void Constructor_ShouldLoadSalesData()
    {
        // Assert
        _viewModel.Sales.Should().HaveCount(_testSales.Count);
        _viewModel.IsExportEnabled.Should().BeTrue(); // Should be enabled when there's data
    }

    [Fact]
    public void Constructor_ShouldCalculateCorrectSummary()
    {
        // Assert
        _viewModel.TotalSales.Should().Be(_testSales.Count);
        _viewModel.BooksSold.Should().Be(_testSales.Sum(s => s.Quantity));
        _viewModel.TotalRevenue.Should().Be(_testSales.Sum(s => s.UnitPrice * s.Quantity));
    }

    [Fact]
    public async Task SelectedDate_WhenChanged_ShouldLoadNewReport()
    {
        // Arrange
        var newDate = DateTime.Today.AddDays(-1);
        var newSales = TestData.CreateTestSales(count: 2);

        _mockBookService.Setup(x => x.GetSalesByDateAsync(newDate)).ReturnsAsync(newSales);

        // Act
        _viewModel.SelectedDate = newDate;
        await Task.Delay(200); // Wait for async load

        // Assert
        _viewModel.SelectedDateText.Should().Be(newDate.ToString("dd-MM-yyyy"));
        _viewModel.Sales.Should().HaveCount(newSales.Count);
        _mockBookService.Verify(x => x.GetSalesByDateAsync(newDate), Times.AtLeastOnce);
    }

    // Note: CSV export functionality is difficult to test in isolation due to
    // SaveFileDialog dependency, but the core CSV generation logic could be
    // refactored into a separate testable method if needed.

    [Theory]
    [InlineData("01-01-2024")]
    [InlineData("31-12-2024")]
    [InlineData("15-06-2023")]
    public async Task SelectedDate_WithDifferentDates_ShouldFormatCorrectly(string dateString)
    {
        // Arrange
        var date = DateTime.Parse(dateString);
        _mockBookService.Setup(x => x.GetSalesByDateAsync(date)).ReturnsAsync(new List<Sale>());

        // Act
        _viewModel.SelectedDate = date;
        await Task.Delay(100);

        // Assert
        _viewModel.SelectedDateText.Should().Be(date.ToString("dd-MM-yyyy"));
    }

    [Fact]
    public void Summary_WithComplexSalesData_ShouldCalculateCorrectly()
    {
        // Arrange
        var complexSales = new List<Sale>
        {
            TestData.CreateTestSale(unitPrice: 10.50, quantity: 2), // 21.00
            TestData.CreateTestSale(unitPrice: 15.75, quantity: 3), // 47.25
            TestData.CreateTestSale(unitPrice: 8.99, quantity: 1), // 8.99
            TestData.CreateTestSale(unitPrice: 25.00, quantity: 4), // 100.00
        };

        _mockBookService
            .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(complexSales);

        // Act
        var viewModel = new SalesReportViewModel(_mockBookService.Object);
        Task.Delay(200).Wait();

        // Assert
        viewModel.TotalSales.Should().Be(4);
        viewModel.BooksSold.Should().Be(10);
        viewModel.TotalRevenue.Should().Be(177.24);
    }
}
