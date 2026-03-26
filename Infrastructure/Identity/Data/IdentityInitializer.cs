using Application.Abstractions.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Data;

internal class IdentityInitializer
{
    public static async Task AddDefaultAdminAsync(IServiceProvider sp)
    {
        await using var scope = sp.CreateAsyncScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

        var result = await authService.SignUpUserAsync("admin@domain.com", "BytMig123!");
    }
}
