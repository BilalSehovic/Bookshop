using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using WpfApp.Commands;

namespace WpfApp.ViewModels;

public class SalesReportingViewModel : ViewModelBase
{
    private readonly ISaleRepository _saleRepository;
    private ObservableCollection<Sale> _salesReport = new();
    private DateTime _selectedDate = DateTime.Today;
    private decimal _totalSales;
    private int _totalTransactions;
    private string _reportSummary = string.Empty;

    public SalesReportingViewModel(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;

        GenerateReportCommand = new RelayCommand(async () => await GenerateReport());
        ExportReportCommand = new RelayCommand(ExportReport, () => SalesReport.Any());
    }

    public ObservableCollection<Sale> SalesReport
    {
        get => _salesReport;
        set => SetProperty(ref _salesReport, value);
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value);
    }

    public decimal TotalSales
    {
        get => _totalSales;
        set => SetProperty(ref _totalSales, value);
    }

    public int TotalTransactions
    {
        get => _totalTransactions;
        set => SetProperty(ref _totalTransactions, value);
    }

    public string ReportSummary
    {
        get => _reportSummary;
        set => SetProperty(ref _reportSummary, value);
    }

    public ICommand GenerateReportCommand { get; }
    public ICommand ExportReportCommand { get; }

    private async Task GenerateReport()
    {
        try
        {
            var sales = await _saleRepository.GetSalesByDateAsync(SelectedDate);
            SalesReport = new ObservableCollection<Sale>(sales);

            TotalSales = sales.Sum(s => (decimal)s.TotalPrice);
            TotalTransactions = sales.Count;

            ReportSummary =
                $"Report for {SelectedDate:dddd, MMMM dd, yyyy}\n"
                + $"Total Sales: {TotalSales:C}\n"
                + $"Total Transactions: {TotalTransactions}\n"
                + $"Books Sold: {sales.Sum(s => s.Quantity)}\n"
                + $"Average Transaction: {(TotalTransactions > 0 ? TotalSales / TotalTransactions : 0):C}";
        }
        catch (Exception ex)
        {
            ReportSummary = $"Error generating report: {ex.Message}";
        }
    }

    private void ExportReport()
    {
        try
        {
            var fileName = $"SalesReport_{SelectedDate:yyyy-MM-dd}.txt";
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                fileName
            );

            var reportContent = new List<string>
            {
                $"BOOKSHOP SALES REPORT",
                $"Date: {SelectedDate:dddd, MMMM dd, yyyy}",
                $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                "",
                "SUMMARY:",
                $"Total Sales: {TotalSales:C}",
                $"Total Transactions: {TotalTransactions}",
                $"Books Sold: {SalesReport.Sum(s => s.Quantity)}",
                $"Average Transaction: {(TotalTransactions > 0 ? TotalSales / TotalTransactions : 0):C}",
                "",
                "DETAILED TRANSACTIONS:",
                "Time\t\tBook\t\tQuantity\tUnit Price\tTotal\t\tCustomer",
            };

            foreach (var sale in SalesReport.OrderBy(s => s.SaleDate))
            {
                var customerName = string.IsNullOrWhiteSpace(sale.CustomerName)
                    ? "Walk-in"
                    : sale.CustomerName;
                reportContent.Add(
                    $"{sale.SaleDate:HH:mm:ss}\t{sale.Book.Title}\t{sale.Quantity}\t\t{sale.UnitPrice:C}\t\t{sale.TotalPrice:C}\t\t{customerName}"
                );
            }

            File.WriteAllLines(filePath, reportContent);
            ReportSummary += $"\n\nReport exported to: {filePath}";
        }
        catch (Exception ex)
        {
            ReportSummary += $"\n\nError exporting report: {ex.Message}";
        }
    }
}
