﻿using FinancialCalculator.Common;

namespace FinancialCalculator.Web.Models;

public class CreditResult
{
    public decimal TotalPayments { get; init; }
    
    public decimal TotalInterest { get; init; }
    
    public decimal TotalPrincipal { get; init; }
    
    public decimal TotalInitialFees { get; init; }
    
    public decimal TotalFees { get; init; }
    
    public decimal AverageMonthlyPayment { get; init; }
    
    public decimal Apr { get; init; }

    public List<AmortizationEntry> AmortizationSchedule { get; init; } = new();
}
