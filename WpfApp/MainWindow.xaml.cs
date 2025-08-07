using System.Windows;
using DataAccessLayer;
using DataAccessLayer.Data;
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
            //var i = personRepository.GetAll();

            InitializeComponent();
            _scopeFactory = scopeFactory;

            using var scope = _scopeFactory.CreateScope();
            var personRepository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var items = context.Persons.ToList();
        }
    }
}
