using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Input;
using DataAccessLayer.Models;
using Microsoft.Win32;
using WpfApp.Interfaces;
using WpfApp.Services;

namespace WpfApp.ViewModels;

public class SalesReportViewModel : ViewModelBase
{
    private readonly IBookService _bookService;
    private readonly IDialogService _dialogService;
    private DateTime _selectedDate = DateTime.Today;
    private ObservableCollection<Sale> _sales = new();
    private int _totalSales = 0;
    private int _booksSold = 0;
    private double _totalRevenue = 0;
    private string _selectedDateText = DateTime.Today.ToString("dd-MM-yyyy");
    private bool _isExportEnabled = false;

    public SalesReportViewModel(IBookService bookService, IDialogService dialogService)
    {
        _bookService = bookService;
        _dialogService = dialogService;
        TodayCommand = new RelayCommand(SetToday);
        ExportCsvCommand = new RelayCommand(ExportCsv, () => IsExportEnabled);
        LoadReportCommand = new RelayCommand(async () => await LoadReportAsync());

        LoadReportAsync();
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (SetProperty(ref _selectedDate, value))
            {
                SelectedDateText = value.ToString("dd-MM-yyyy");
                LoadReportAsync();
            }
        }
    }

    public ObservableCollection<Sale> Sales
    {
        get => _sales;
        set => SetProperty(ref _sales, value);
    }

    public int TotalSales
    {
        get => _totalSales;
        set => SetProperty(ref _totalSales, value);
    }

    public int BooksSold
    {
        get => _booksSold;
        set => SetProperty(ref _booksSold, value);
    }

    public double TotalRevenue
    {
        get => _totalRevenue;
        set => SetProperty(ref _totalRevenue, value);
    }

    public string SelectedDateText
    {
        get => _selectedDateText;
        set => SetProperty(ref _selectedDateText, value);
    }

    public bool IsExportEnabled
    {
        get => _isExportEnabled;
        set
        {
            if (SetProperty(ref _isExportEnabled, value))
            {
                ((RelayCommand)ExportCsvCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand TodayCommand { get; }
    public ICommand ExportCsvCommand { get; }
    public ICommand LoadReportCommand { get; }

    private void SetToday()
    {
        SelectedDate = DateTime.Today;
    }

    private async Task LoadReportAsync()
    {
        try
        {
            var sales = await _bookService.GetSalesByDateAsync(SelectedDate);

            Sales.Clear();
            foreach (var sale in sales)
            {
                Sales.Add(sale);
            }

            UpdateSummary(sales.ToList());
            IsExportEnabled = sales.Any();
        }
        catch (Exception ex)
        {
            _dialogService.ShowError($"Error loading sales report: {ex.Message}", "Error");
            ClearReport();
        }
    }

    private void UpdateSummary(List<Sale> sales)
    {
        TotalSales = sales.Count;
        BooksSold = sales.Sum(s => s.Quantity);
        TotalRevenue = sales.Sum(s => s.UnitPrice * s.Quantity);
    }

    private void ClearReport()
    {
        Sales.Clear();
        TotalSales = 0;
        BooksSold = 0;
        TotalRevenue = 0;
        IsExportEnabled = false;
    }

    private void ExportCsv()
    {
        if (!Sales.Any())
        {
            _dialogService.ShowError("No sales data to export.", "No Data");
            return;
        }

        var defaultFileName = $"SalesReport_{SelectedDateText}.csv";

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            FileName = defaultFileName,
            DefaultExt = "csv",
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                ExportToCsv(saveFileDialog.FileName);
                _dialogService.ShowInformation(
                    $"Sales report exported successfully to:\n{saveFileDialog.FileName}",
                    "Export Successful"
                );
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Error exporting CSV: {ex.Message}", "Export Error");
            }
        }
    }

    private void ExportToCsv(string filePath)
    {
        var csv = new StringBuilder();

        csv.AppendLine("Book Title,Author,ISBN,Sale Price,Sale Date,Sale Time,Quantity,Total");

        foreach (var sale in Sales)
        {
            var bookTitle = EscapeCsvField(sale.Book?.Title ?? "");
            var author = EscapeCsvField(sale.Book?.Author ?? "");
            var isbn = EscapeCsvField(sale.Book?.Isbn ?? "");
            var salePrice = sale.UnitPrice.ToString("F2", CultureInfo.InvariantCulture);
            var localSaleDate = sale.SaleDate.ToLocalTime();
            var saleDate = localSaleDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            var saleTime = localSaleDate.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            var quantity = sale.Quantity.ToString(CultureInfo.InvariantCulture);
            var total = (sale.UnitPrice * sale.Quantity).ToString(
                "F2",
                CultureInfo.InvariantCulture
            );

            csv.AppendLine(
                $"{bookTitle},{author},{isbn},{salePrice},{saleDate},{saleTime},{quantity},{total}"
            );
        }

        File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
    }

    private string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return "";

        if (
            field.Contains(',')
            || field.Contains('"')
            || field.Contains('\n')
            || field.Contains('\r')
        )
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }

        return field;
    }

    public void RefreshReport()
    {
        LoadReportAsync();
    }
}
