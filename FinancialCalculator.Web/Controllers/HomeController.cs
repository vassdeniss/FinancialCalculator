using System.Diagnostics;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using Microsoft.AspNetCore.Mvc;
using FinancialCalculator.Web.Models;

namespace FinancialCalculator.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICreditService _creditService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ICreditService creditService, ILogger<HomeController> _logger)
    {
        this._creditService = creditService;
        this._logger = _logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
    
    [HttpPost]
    public IActionResult Calculate(CreditInputDto input)
    {
        try
        {

            if (!this.ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                
                return this.View("Error");
            }
        
            CreditResultDto creditResultDto = this._creditService.CalculateCreditResult(input);
        
            CreditResult creditResult = new()
            {
                TotalPayments = Math.Round(creditResultDto.TotalPayments, 2),      
                TotalInterest = Math.Round(creditResultDto.TotalInterest, 2),     
                TotalPrincipal = Math.Round(creditResultDto.TotalPrincipal, 2),
                TotalInitialFees = Math.Round(creditResultDto.TotalInitialFees, 2),
                TotalFees = Math.Round(creditResultDto.TotalFees, 2),
                AverageMonthlyPayment = Math.Round(creditResultDto.AverageMonthlyPayment, 2),
                AmortizationSchedule = creditResultDto.AmortizationSchedule,
                Apr = Math.Round(creditResultDto.Apr, 2)
            };

            return this.View("Result", creditResult);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "An error occured");
            return this.RedirectToAction("Index", "Home");
        }
        
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}