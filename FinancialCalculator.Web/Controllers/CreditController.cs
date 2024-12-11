using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialCalculator.Web.Controllers;

public class CreditController : Controller
{
    private readonly ICreditService _creditService;

    public CreditController(ICreditService creditService)
    {
        this._creditService = creditService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
    
    
    [HttpPost]
    public IActionResult Calculate(CreditInputDto input)
    {
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
}
