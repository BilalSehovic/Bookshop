using DataAccessLayer;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IBookRepository, BookRepository>();

        services.AddScoped<MainWindow>();
    }
}
