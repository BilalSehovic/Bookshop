using System.IO;
using WpfApp.Services;
using WpfApp.Tests.TestHelpers;
using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels
{
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
        public void Constructor_ShouldInitializeWithTodayAndLoadReport()
        {
            // Assert
            _viewModel.SelectedDate.Date.Should().Be(DateTime.Today);
            _viewModel.SelectedDateText.Should().Be(DateTime.Today.ToString("yyyy-MM-dd"));
            _mockBookService.Verify(
                x => x.GetSalesByDateAsync(It.IsAny<DateTime>()),
                Times.AtLeastOnce
            );
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
            _viewModel.SelectedDateText.Should().Be(newDate.ToString("yyyy-MM-dd"));
            _viewModel.Sales.Should().HaveCount(newSales.Count);
            _mockBookService.Verify(x => x.GetSalesByDateAsync(newDate), Times.AtLeastOnce);
        }

        [Fact]
        public void TodayCommand_ShouldSetSelectedDateToToday()
        {
            // Arrange
            _viewModel.SelectedDate = DateTime.Today.AddDays(-5);

            // Act
            _viewModel.TodayCommand.Execute(null);

            // Assert
            _viewModel.SelectedDate.Date.Should().Be(DateTime.Today);
        }

        [Fact]
        public void ExportCsvCommand_CanExecute_ShouldReturnCorrectValue()
        {
            // Arrange & Act & Assert
            _viewModel.IsExportEnabled = true;
            _viewModel.ExportCsvCommand.CanExecute(null).Should().BeTrue();

            _viewModel.IsExportEnabled = false;
            _viewModel.ExportCsvCommand.CanExecute(null).Should().BeFalse();
        }

        [Fact]
        public void IsExportEnabled_WhenChanged_ShouldRaiseCanExecuteChanged()
        {
            // Arrange
            var canExecuteChanged = false;
            _viewModel.ExportCsvCommand.CanExecuteChanged += (sender, args) =>
                canExecuteChanged = true;

            // Act
            _viewModel.IsExportEnabled = !_viewModel.IsExportEnabled;

            // Assert
            canExecuteChanged.Should().BeTrue();
        }

        [Fact]
        public async Task LoadReportAsync_WithNoSales_ShouldDisableExport()
        {
            // Arrange
            _mockBookService
                .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Sale>());

            var viewModel = new SalesReportViewModel(_mockBookService.Object);
            await Task.Delay(200);

            // Assert
            viewModel.Sales.Should().BeEmpty();
            viewModel.TotalSales.Should().Be(0);
            viewModel.BooksSold.Should().Be(0);
            viewModel.TotalRevenue.Should().Be(0);
            viewModel.IsExportEnabled.Should().BeFalse();
        }

        [Fact]
        public async Task LoadReportAsync_WhenServiceThrows_ShouldHandleGracefully()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            mockService
                .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Service error"));

            var viewModel = new SalesReportViewModel(mockService.Object);
            await Task.Delay(200);

            // Assert
            viewModel.Sales.Should().BeEmpty();
            viewModel.IsExportEnabled.Should().BeFalse();
        }

        [Fact]
        public async Task RefreshReport_ShouldReloadCurrentDate()
        {
            // Arrange
            var newSales = TestData.CreateTestSales(count: 5);
            _mockBookService
                .Setup(x => x.GetSalesByDateAsync(_viewModel.SelectedDate))
                .ReturnsAsync(newSales);

            // Act
            _viewModel.RefreshReport();
            await Task.Delay(200);

            // Assert
            _viewModel.Sales.Should().HaveCount(newSales.Count);
            _mockBookService.Verify(
                x => x.GetSalesByDateAsync(_viewModel.SelectedDate),
                Times.AtLeast(2)
            );
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
            _viewModel.TotalSales = 10;
            _viewModel.BooksSold = 25;
            _viewModel.TotalRevenue = 199.99;
            _viewModel.SelectedDateText = "2024-01-01";
            _viewModel.IsExportEnabled = true;

            // Assert
            propertyChangedEvents.Should().Contain(nameof(_viewModel.TotalSales));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.BooksSold));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.TotalRevenue));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.SelectedDateText));
            propertyChangedEvents.Should().Contain(nameof(_viewModel.IsExportEnabled));
        }

        [Fact]
        public void Sales_ObservableCollection_ShouldNotifyChanges()
        {
            // Arrange
            var collectionChanged = false;
            _viewModel.Sales.CollectionChanged += (sender, args) => collectionChanged = true;

            // Act
            _viewModel.Sales.Add(TestData.CreateTestSale());

            // Assert
            collectionChanged.Should().BeTrue();
        }

        // Note: CSV export functionality is difficult to test in isolation due to
        // SaveFileDialog dependency, but the core CSV generation logic could be
        // refactored into a separate testable method if needed.

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-12-31")]
        [InlineData("2023-06-15")]
        public async Task SelectedDate_WithDifferentDates_ShouldFormatCorrectly(string dateString)
        {
            // Arrange
            var date = DateTime.Parse(dateString);
            _mockBookService.Setup(x => x.GetSalesByDateAsync(date)).ReturnsAsync(new List<Sale>());

            // Act
            _viewModel.SelectedDate = date;
            await Task.Delay(100);

            // Assert
            _viewModel.SelectedDateText.Should().Be(date.ToString("yyyy-MM-dd"));
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
            viewModel.BooksSold.Should().Be(10); // 2+3+1+4
            viewModel.TotalRevenue.Should().Be(177.24); // 21.00+47.25+8.99+100.00
        }

        [Fact]
        public void LoadReportCommand_ShouldTriggerReportLoad()
        {
            // Arrange
            var newSales = TestData.CreateTestSales(count: 1);
            _mockBookService
                .Setup(x => x.GetSalesByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(newSales);

            // Act
            _viewModel.LoadReportCommand.Execute(null);
            Task.Delay(200).Wait();

            // Assert
            _mockBookService.Verify(
                x => x.GetSalesByDateAsync(It.IsAny<DateTime>()),
                Times.AtLeast(2)
            );
        }
    }
}
