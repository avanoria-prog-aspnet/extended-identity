using Application.Abstractions.Authentication;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Identity.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(environment);

        services.AddIdentity<AppUser, AppRole>(x =>
        {
            x.SignIn.RequireConfirmedAccount = false;
            x.User.RequireUniqueEmail = true;
            x.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<PersistenceContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(x =>
        {
            var cookieName = configuration?.GetValue<string>("CookieSettings:CookieName") ?? "corefitness.auth";
            var loginPath = configuration?.GetValue<string>("CookieSettings:LoginPath") ?? "/sign-in";
            var logoutPath = configuration?.GetValue<string>("CookieSettings:LogoutPath") ?? "/";
            var deniedPath = configuration?.GetValue<string>("CookieSettings:DeniedPath") ?? "/denied";
            var maxAgeInDays = configuration?.GetValue<int>("CookieSettings:MaxAgeInDays") ?? 90;
            var expiresInDays = configuration?.GetValue<int>("CookieSettings:ExpiresInDays") ?? 30;

            x.Cookie.IsEssential = true;

            x.Cookie.Name = cookieName;
            x.LoginPath = loginPath;
            x.LogoutPath = logoutPath;
            x.AccessDeniedPath = deniedPath;
            x.Cookie.MaxAge = TimeSpan.FromDays(maxAgeInDays);
            x.ExpireTimeSpan = TimeSpan.FromDays(expiresInDays);
        });

        services.AddScoped<IAuthService, IdentityAuthService>();

        return services;
    }
}
