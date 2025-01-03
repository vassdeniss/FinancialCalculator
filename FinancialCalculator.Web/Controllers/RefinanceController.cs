using AutoMapper;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;
using RefinanceInputDto = FinancialCalculator.Web.Dto.RefinanceInputDto;

namespace FinancialCalculator.Web.Controllers;

public class RefinanceController : Controller
{
    private readonly IRefinanceService _refinanceService;
    private readonly IMapper _mapper;

    public RefinanceController(IRefinanceService refinanceService, IMapper mapper)
    {
        this._refinanceService = refinanceService;
        this._mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
    
    [HttpPost]
    public IActionResult Calculate(RefinanceInputDto input)
    {
        if (!this.ModelState.IsValid)
        {
            this.TempData["ShowError"] = true;
            return this.View(nameof(Index), input);
        }
        
        RefinanceServiceInputDto dto = this._mapper.Map<RefinanceServiceInputDto>(input);
        RefinanceResultDto result = this._refinanceService.Calculate(dto);

        RefinanceResult refinanceResult = new()
        {
            CurrentAnnualInterestRate = input.AnnualInterestRate,
            NewAnnualInterestRate = input.NewAnnualInterestRate,
            LoanTerm = input.LoanTermInMonths,
            NewLoanTerm = input.LoanTermInMonths - input.ContributionsMade,
            CurrentMonthlyInstallment = result.CurrentMonthlyInstallment.Round(2),
            CurrentTotalPaid = result.CurrentTotalPaid.Round(2),
            CurrentEarlyRepaymentFee = result.CurrentEarlyRepaymentFee.Round(2),
            NewMonthlyInstallment = result.NewMonthlyInstallment.Round(2),
            NewTotalPaid = result.NewTotalPaid.Round(2),
            NewInitialFees = result.NewInitialFees.Round(2),
            SavingsDifference = result.SavingsDifference.Round(2),
            Message = result.Message
        };
        
        return this.View("Result", refinanceResult);
    }
}
