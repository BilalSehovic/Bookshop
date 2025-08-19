using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Interfaces;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;
    private UserControl? _currentContent;
    private string _windowTitle = "Bookshop Management System";
    private BookManagementView? _bookManagementView;
    private SalesView? _salesView;
    private SalesReportView? _salesReportView;

    public MainWindowViewModel(IServiceProvider serviceProvider, IDialogService dialogService)
    {
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
        LoadBookManagementCommand = new RelayCommand(LoadBookManagement);
        LoadSalesCommand = new RelayCommand(LoadSales);
        LoadSalesReportCommand = new RelayCommand(LoadSalesReport);
        InitializeDatabaseCommand = new RelayCommand(async () => await InitializeDatabaseAsync());
        ShowAboutCommand = new RelayCommand(ShowAbout);

        // Load initial view
        LoadBookManagement();
    }

    public UserControl? CurrentContent
    {
        get => _currentContent;
        set => SetProperty(ref _currentContent, value);
    }

    public string WindowTitle
    {
        get => _windowTitle;
        set => SetProperty(ref _windowTitle, value);
    }

    public ICommand LoadBookManagementCommand { get; }
    public ICommand LoadSalesCommand { get; }
    public ICommand LoadSalesReportCommand { get; }
    public ICommand InitializeDatabaseCommand { get; }
    public ICommand ShowAboutCommand { get; }

    private void LoadBookManagement()
    {
        if (_bookManagementView == null)
            _bookManagementView = _serviceProvider.GetRequiredService<BookManagementView>();
        else
            _bookManagementView.RefreshView();

        CurrentContent = _bookManagementView;
        WindowTitle = "Bookshop Management System - Book Management";
    }

    private void LoadSales()
    {
        if (_salesView == null)
            _salesView = _serviceProvider.GetRequiredService<SalesView>();
        else
            _salesView.RefreshView();

        CurrentContent = _salesView;
        WindowTitle = "Bookshop Management System - Sales";
    }

    private void LoadSalesReport()
    {
        if (_salesReportView == null)
            _salesReportView = _serviceProvider.GetRequiredService<SalesReportView>();
        else
            _salesReportView.RefreshReport();

        CurrentContent = _salesReportView;
        WindowTitle = "Bookshop Management System - Sales Report";
    }

    private async Task InitializeDatabaseAsync()
    {
        var result = _dialogService.ShowConfirmation(
            "This will initialize the database and create sample data. Continue?",
            "Initialize Database"
        );

        if (result)
        {
            try
            {
                var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.EnsureCreatedAsync();

                if (!dbContext.Books.Any())
                {
                    await AddSampleData(dbContext);
                }

                _dialogService.ShowInformation("Database initialized successfully!", "Success");

                // Reset views to refresh with new data
                _bookManagementView = null;
                _salesView = null;
                _salesReportView = null;
                LoadBookManagement();
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Error initializing database: {ex.Message}", "Error");
            }
        }
    }

    private async Task AddSampleData(AppDbContext dbContext)
    {
        var sampleBooks = AppDbSeeded.GetSeedBooks();

        dbContext.Books.AddRange(sampleBooks);
        await dbContext.SaveChangesAsync();
    }

    private void ShowAbout()
    {
        _dialogService.ShowInformation(
            "Bookshop Management System\n\nVersion 1.0\n\nA simple WPF application for managing book inventory and sales.\n\nFeatures:\n• Book Management (Add, Edit, Delete)\n• Sales Processing\n• Sales Reporting\n\nBuilt with C# WPF, Entity Framework Core, and PostgreSQL.",
            "About Bookshop Management System"
        );
    }
}
