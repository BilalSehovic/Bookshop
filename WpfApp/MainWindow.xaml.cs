using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MainWindow(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            InitializeComponent();
        }
    }
}
