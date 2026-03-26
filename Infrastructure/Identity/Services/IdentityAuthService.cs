using Application.Abstractions.Authentication;
using Application.Common.Results;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public sealed class IdentityAuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IAuthService
{
    public async Task<AuthResult> SignUpUserAsync(string email, string password, string? roleName = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.Failed("Invalid credentials provided.");

        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
            return AuthResult.Failed("User with same email address already exists");

        user = AppUser.Create(email);

        var created = await userManager.CreateAsync(user, password);
        if (!created.Succeeded)
            return AuthResult.Failed(created.Errors.FirstOrDefault()?.Description ?? "Unable to create user");

        return AuthResult.Ok();
    }

    public async Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.Failed("Incorrect email address or password.");

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return AuthResult.Failed("Incorrect email address or password.");

        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        
        if (result.IsLockedOut)
            return AuthResult.Failed("User has been locked. Please contact support.");

        if (result.IsNotAllowed)
            return AuthResult.Failed("Unable to login. Please contact support.");

        if (result.RequiresTwoFactor)
            return AuthResult.Failed("This user requires multi-factor authentication.");

        if (!result.Succeeded)
            return AuthResult.Failed("Incorrect email address or password.");

        return AuthResult.Ok();
    }

    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}
