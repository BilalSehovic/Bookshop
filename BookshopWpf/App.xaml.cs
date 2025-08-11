using System.Windows;
using BookshopWpf.Data;
using BookshopWpf.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookshopWpf
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(
                    (context, services) =>
                    {
                        var connectionString =
                            "Host=localhost;Port=5432;Username=postgres;Password=1;Database=postgres;";

                        services.AddDbContext<BookshopDbContext>(options =>
                            options.UseNpgsql(connectionString)
                        );

                        services.AddScoped<IBookService, BookService>();
                        services.AddTransient<MainWindow>();
                    }
                )
                .Build();

            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }
}
