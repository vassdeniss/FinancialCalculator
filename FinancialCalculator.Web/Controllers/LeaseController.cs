using AutoMapper;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Web.Dto;
using FinancialCalculator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialCalculator.Web.Controllers;

public class LeaseController : Controller
{
    private readonly ILeaseService _leaseService;
    private readonly IMapper _mapper;
    
    public LeaseController(ILeaseService leaseService, IMapper mapper)
    {
        this._leaseService = leaseService;
        this._mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
    
    [HttpPost]
    public IActionResult Calculate(LeaseInputDto input)
    {
        if (!this.ModelState.IsValid)
        {
            this.TempData["ShowError"] = true;
            return this.View(nameof(Index), input);
        }
        
        LeaseServiceInputDto dto = this._mapper.Map<LeaseServiceInputDto>(input);
        LeaseResultDto leaseResultDto = this._leaseService.CalculateLeaseResult(dto);
    
        LeaseResult leaseResult = new()
        {
            AnnualCostPercent = leaseResultDto.AnnualCostPercent.Round(2),      
            TotalPaid = leaseResultDto.TotalPaid.Round(2),      
            TotalFees = leaseResultDto.TotalFees.Round(2)
        };
    
        return this.View("Result", leaseResult);
    }
}
