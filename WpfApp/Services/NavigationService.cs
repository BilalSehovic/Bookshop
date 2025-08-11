using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MainWindow _mainWindow;

    // Keep track of the current scope
    private IServiceScope _currentScope;

    public NavigationService(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _serviceProvider = serviceProvider;
        _mainWindow = mainWindow;
    }

    public void Navigate<TView>()
        where TView : UserControl
    {
        // Dispose the old scope if any
        _currentScope?.Dispose();

        // Create a new scope for this view
        _currentScope = _serviceProvider.CreateScope();

        // Resolve the view from this scope
        var view = _currentScope.ServiceProvider.GetRequiredService<TView>();

        _mainWindow.CurrentView = view;
    }
}
