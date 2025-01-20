using FinancialCalculator.Common;

namespace FinancialCalculator.Web.Models;

public class LeaseResult
{
    public BigDecimal AnnualCostPercent { get; init; }
    
    public BigDecimal TotalPaid { get; init; }
    
    public BigDecimal TotalFees { get; init; }
}
