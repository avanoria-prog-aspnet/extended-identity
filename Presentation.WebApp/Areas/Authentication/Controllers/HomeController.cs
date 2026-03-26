using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
public class HomeController(IAuthService authService) : Controller
{
    [HttpGet("registration/sign-up")]
    public IActionResult Index()
    {
        return View();
    }


    [HttpPost("registration/sign-up")]
    public async Task<IActionResult> Index(SignUpForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        HttpContext.Session.SetString("reg_email", form.Email);
        return RedirectToAction(nameof(SetPassword));
    }


    [HttpGet("registration/set-password")]
    public IActionResult SetPassword()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.Session?.GetString("reg_email")))
            return RedirectToAction(nameof(Index));

        return View();
    }


    [HttpPost("registration/set-password")]
    public async Task<IActionResult> SetPassword(SetPasswordForm form)
    {
        var email = HttpContext.Session?.GetString("reg_email");

        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(Index));

        if (!ModelState.IsValid)
            return View(form);

        var created = await authService.SignUpUserAsync(email, form.Password);
        if (!created.Succeeded)
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), created.ErrorMessage ?? "Unable to create user");
            return View(form);
        }

        var result = await authService.SignInUserAsync(email, form.Password, false);
        if (!result.Succeeded)
            return RedirectToAction(nameof(SignIn));

        return Redirect("/me");
    }





    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        ViewData["Title"] = "Sign In";

        return View();
    }
}
