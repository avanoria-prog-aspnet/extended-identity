using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
public class HomeController : Controller
{
    [HttpGet("registration/sign-up")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Become a Member";

        return View();
    }

    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        ViewData["Title"] = "Sign In";

        return View();
    }
}
