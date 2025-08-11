using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class SalesReportingView : UserControl
{
    public SalesReportingView(SalesReportingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}