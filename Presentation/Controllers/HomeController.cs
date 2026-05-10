using Microsoft.AspNetCore.Mvc;

namespace docs_project.Presentation.Controllers;

public class HomeController : Controller
{
    [HttpGet("Home")]
    [HttpGet("Home/Index")]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "ChatsMvc");
        }

        return RedirectToAction("Login", "Account");
    }
}

