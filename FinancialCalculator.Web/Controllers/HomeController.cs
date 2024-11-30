using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinancialCalculator.Web.Models;

namespace FinancialCalculator.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    [HttpPost]
    public IActionResult Calculate(FormCollection form)
    {
        decimal loanAmount;
        int loanTerm;
        double annualInterestRate;
        int promotionalPeriod;
        double annualPromotionalInterestRate;
        int gracePeriod;
        string paymentType;

        decimal applicationFeeValue = 0m;
        string applicationFeeType = "amount";

        decimal processingFeeValue = 0m;
        string processingFeeType = "amount";

        decimal otherInitialFeesValue = 0m;
        string otherInitialFeesType = "amount";

        decimal monthlyManagementFeeValue = 0m;
        string monthlyManagementFeeType = "amount";

        decimal otherMonthlyFeesValue = 0m;
        string otherMonthlyFeesType = "amount";

        decimal annualManagementFeeValue = 0m;
        string annualManagementFeeType = "amount";

        decimal otherAnnualFeesValue = 0m;
        string otherAnnualFeesType = "amount";

        // Retrieve and parse each field individually

        if (!decimal.TryParse(form["loanAmount"], out loanAmount))
        {
            loanAmount = 0m;
        }

        if (!int.TryParse(form["loanTerm"], out loanTerm))
        {
            loanTerm = 0;
        }

        if (!double.TryParse(form["annualInterestRate"], out annualInterestRate))
        {
            annualInterestRate = 0.0;
        }

        if (!int.TryParse(form["promotionalPeriod"], out promotionalPeriod))
        {
            promotionalPeriod = 0;
        }

        if (!double.TryParse(form["annualPromotionalInterestRate"], out annualPromotionalInterestRate))
        {
            annualPromotionalInterestRate = 0.0;
        }

        if (!int.TryParse(form["gracePeriod"], out gracePeriod))
        {
            gracePeriod = 0;
        }

        paymentType = form["paymentType"]!;
        
        if (!decimal.TryParse(form["applicationFee"], out applicationFeeValue))
        {
            applicationFeeValue = 0m;
        }

        if (!decimal.TryParse(form["processingFee"], out processingFeeValue))
        {
            processingFeeValue = 0m;
        }

        if (!decimal.TryParse(form["otherInitialFees"], out otherInitialFeesValue))
        {
            otherInitialFeesValue = 0m;
        }

        if (!decimal.TryParse(form["monthlyManagementFee"], out monthlyManagementFeeValue))
        {
            monthlyManagementFeeValue = 0m;
        }

        if (!decimal.TryParse(form["otherMonthlyFees"], out otherMonthlyFeesValue))
        {
            otherMonthlyFeesValue = 0m;
        }

        if (!decimal.TryParse(form["annualManagementFee"], out annualManagementFeeValue))
        {
            annualManagementFeeValue = 0m;
        }

        if (!decimal.TryParse(form["otherAnnualFees"], out otherAnnualFeesValue))
        {
            otherAnnualFeesValue = 0m;
        }

        
        Console.WriteLine($"Loan Amount: {loanAmount}");
        Console.WriteLine($"Loan Term: {loanTerm}");
        Console.WriteLine($"Annual Interest Rate: {annualInterestRate}");
        Console.WriteLine($"Promotional Period: {promotionalPeriod}");
        Console.WriteLine($"Annual Promotional Interest Rate: {annualPromotionalInterestRate}");
        Console.WriteLine($"Grace Period: {gracePeriod}");
        Console.WriteLine($"Payment Type: {paymentType}");

        Console.WriteLine($"Application Fee: {applicationFeeValue} ({applicationFeeType})");
        Console.WriteLine($"Processing Fee: {processingFeeValue} ({processingFeeType})");
        Console.WriteLine($"Other Initial Fees: {otherInitialFeesValue} ({otherInitialFeesType})");

        Console.WriteLine($"Monthly Management Fee: {monthlyManagementFeeValue} ({monthlyManagementFeeType})");
        Console.WriteLine($"Other Monthly Fees: {otherMonthlyFeesValue} ({otherMonthlyFeesType})");

        Console.WriteLine($"Annual Management Fee: {annualManagementFeeValue} ({annualManagementFeeType})");
        Console.WriteLine($"Other Annual Fees: {otherAnnualFeesValue} ({otherAnnualFeesType})");

        // For demonstration purposes, redirect back to the form
        return this.RedirectToAction("LoanCalculator");
    }
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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