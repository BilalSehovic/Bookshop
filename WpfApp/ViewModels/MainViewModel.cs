using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.Commands;
using WpfApp.Views;

namespace WpfApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private IServiceScope _currentScope;
    private UserControl? _currentView;
    private string _currentViewName = "Welcome";

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        ShowBookManagementCommand = new RelayCommand(
            () => ShowView<BookManagementView>("Book Management")
        );
        ShowSalesCommand = new RelayCommand(() => ShowView<SalesView>("Sales"));
        ShowSalesReportingCommand = new RelayCommand(
            () => ShowView<SalesReportingView>("Sales Reporting")
        );
    }

    public UserControl? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public string CurrentViewName
    {
        get => _currentViewName;
        set => SetProperty(ref _currentViewName, value);
    }

    public ICommand ShowBookManagementCommand { get; }
    public ICommand ShowSalesCommand { get; }
    public ICommand ShowSalesReportingCommand { get; }

    private void ShowView<T>(string viewName)
        where T : UserControl
    {
        // Dispose previous scope (and its services)
        _currentScope?.Dispose();

        // Create a new scope for the new view
        _currentScope = _serviceProvider.CreateScope();

        // Resolve view from the scoped provider
        var view = _currentScope.ServiceProvider.GetRequiredService<T>();

        CurrentView = view;
        CurrentViewName = viewName;
    }
}
