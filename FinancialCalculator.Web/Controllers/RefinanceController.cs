using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RefinanceInputDto = FinancialCalculator.Web.Dto.RefinanceInputDto;

namespace FinancialCalculator.Web.Controllers;

public class RefinanceController : Controller
{
    private readonly IMapper _mapper;

    public RefinanceController(IMapper mapper)
    {
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
        
        return this.View("Result");
    }
}
