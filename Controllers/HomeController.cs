using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Contacts_V2.Models;

namespace Contacts_V2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet("/")]
    public IActionResult Index()
    {
        if( HttpContext.Session.GetInt32("ID") > 0){
            return LocalRedirect("/Contact/Index");
        }
        return LocalRedirect("/Auth/Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
