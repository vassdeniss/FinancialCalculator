using Microsoft.AspNetCore.Mvc;

namespace FinancialCalculator.Web.Controllers;

public class LeasingController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
}
