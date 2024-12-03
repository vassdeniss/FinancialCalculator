namespace FinancialCalculator.Common;

public class AmortizationEntry
{
    public int Month { get; init; }
    
    public string Date { get; init; }
    
    public decimal Payment { get; init; }
    
    public decimal Principal { get; init; }
    
    public decimal Interest { get; init; }
    
    public decimal RemainingBalance { get; init; }
    
    public decimal Fees { get; init; }
}