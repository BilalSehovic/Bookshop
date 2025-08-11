using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class SalesView : UserControl
{
    public SalesView(SalesViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.LoadBooksCommand.Execute(null);
    }
}