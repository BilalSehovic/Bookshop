using System.Windows;
using WpfApp.Services;

namespace WpfApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //public MainWindow(MainViewModel viewModel)
    //{
    //    InitializeComponent();
    //    DataContext = viewModel;
    //}
    public MainWindow(INavigationService navigation)
    {
        InitializeComponent();

        // Start with Home view
        navigation.Navigate<HomeView>();
    }
}
