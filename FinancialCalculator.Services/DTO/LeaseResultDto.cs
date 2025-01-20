using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class LeaseResultDto
{
    public BigDecimal AnnualCostPercent { get; init; }
    
    public BigDecimal TotalPaid { get; init; }
    
    public BigDecimal TotalFees { get; init; }
}
