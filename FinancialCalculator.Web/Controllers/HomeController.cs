using System.Diagnostics;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using FinancialCalculator.Web.Models;

namespace FinancialCalculator.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }

    [HttpGet]
    public IActionResult About()
    {
        return this.View();
    }
    
    [HttpGet]
    public IActionResult Contact()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
