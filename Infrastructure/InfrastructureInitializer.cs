using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class InfrastructureInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);



        // Initialize database

        // Initialize default roles

        // Initialize default admin account
        await IdentityInitializer.AddDefaultAdminAsync(serviceProvider);
    }
}
