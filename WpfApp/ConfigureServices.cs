using DataAccessLayer;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp;

public static class ConfigureServices
{
    public static void AddServices(HostBuilderContext context, IServiceCollection services)
    {
        IConfiguration config = context.Configuration;

        // Connection string from appsettings or user secrets
        var connectionString = config.GetConnectionString("DefaultConnection");

        // DbContext
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        // Repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();

        // ViewModels
        services.AddScoped<MainViewModel>();
        services.AddScoped<BookManagementViewModel>();
        services.AddScoped<SalesViewModel>();
        services.AddScoped<SalesReportingViewModel>();

        // Views
        services.AddTransient<BookManagementView>();
        services.AddTransient<SalesView>();
        services.AddTransient<SalesReportingView>();

        // Main Window
        services.AddSingleton<MainWindow>();
    }
}
