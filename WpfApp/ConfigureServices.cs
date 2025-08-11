using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp;

public static class ConfigureServices
{
    public static void AddServices(HostBuilderContext context, IServiceCollection services)
    {
        // Connection string from appsettings or user secrets
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        // DbContext
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // Services
        services.AddScoped<IBookService, BookService>();

        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<BookManagementViewModel>();
        services.AddTransient<SalesReportViewModel>();
        services.AddTransient<SalesViewModel>();

        // Views
        services.AddTransient<BookManagementView>();
        services.AddTransient<SalesReportView>();
        services.AddTransient<SalesView>();
        services.AddTransient<MainWindow>();
    }
}
