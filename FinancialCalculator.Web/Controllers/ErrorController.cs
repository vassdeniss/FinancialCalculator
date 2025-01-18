using FinancialCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialCalculator.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [Route("")]
    [Route("{statusCode:int}")]
    public IActionResult HandleError(int? statusCode = null)
    {
        ErrorViewModel model = new()
        {
            StatusCode = statusCode
        };
        
        return this.View("Error", model);
    }
}
