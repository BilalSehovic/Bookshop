using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class BookManagementView : UserControl
{
    public BookManagementView(BookManagementViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public void RefreshView()
    {
        if (DataContext is BookManagementViewModel viewModel)
        {
            viewModel.RefreshView();
        }
    }
}
