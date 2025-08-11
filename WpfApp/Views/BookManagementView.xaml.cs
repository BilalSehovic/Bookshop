using System.Windows.Controls;
using WpfApp.ViewModels;

namespace WpfApp.Views;

public partial class BookManagementView : UserControl
{
    public BookManagementView(BookManagementViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.LoadBooksCommand.Execute(null);
    }
}