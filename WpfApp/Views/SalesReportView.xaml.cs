using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class SalesReportView : UserControl
{
    public SalesReportView(SalesReportViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public void RefreshReport()
    {
        if (DataContext is SalesReportViewModel viewModel)
        {
            viewModel.RefreshReport();
        }
    }
}
