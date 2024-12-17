namespace FinancialCalculator.Common;

public class AmortizationEntry
{
    public int Month { get; init; }
    
    public string Date { get; init; }
    
    public BigDecimal Payment { get; init; }
    
    public BigDecimal Principal { get; init; }
    
    public BigDecimal Interest { get; init; }
    
    public BigDecimal RemainingBalance { get; init; }
    
    public BigDecimal Fees { get; init; }
}