using AutoMapper;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Dto;
using FinancialCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialCalculator.Web.Controllers;

public class CreditController : Controller
{
    private readonly ICreditService _creditService;
    private readonly IMapper _mapper;

    public CreditController(ICreditService creditService, IMapper mapper)
    {
        this._creditService = creditService;
        this._mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
    
    [HttpPost]
    public IActionResult Calculate(CreditInputDto input)
    {
        if (!this.ModelState.IsValid)
        {
            this.TempData["ShowError"] = true;
            return this.View(nameof(Index), input);
        }
        
        CreditServiceInputDto dto = this._mapper.Map<CreditServiceInputDto>(input);
        CreditResultDto creditResultDto = this._creditService.CalculateCreditResult(dto);
    
        CreditResult creditResult = new()
        {
            TotalPayments = creditResultDto.TotalPayments.Round(2),      
            TotalInterest = creditResultDto.TotalInterest.Round(2),  
            TotalPrincipal = creditResultDto.TotalPrincipal.Round(2),
            TotalInitialFees = creditResultDto.TotalInitialFees.Round(2),
            TotalFees = creditResultDto.TotalFees.Round(2),
            AverageMonthlyPayment = creditResultDto.AverageMonthlyPayment.Round(2),
            AmortizationSchedule = creditResultDto.AmortizationSchedule,
            Apr = creditResultDto.Apr.Round(2)
        };
    
        return this.View("Result", creditResult);
    }
}
