using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class CreditResultDto
{
    public BigDecimal TotalPayments { get; init; }
    
    public BigDecimal TotalInterest { get; init; }
    
    public BigDecimal TotalPrincipal { get; init; }
    
    public BigDecimal TotalInitialFees { get; init; }
    
    public BigDecimal TotalFees { get; init; }
    
    public BigDecimal AverageMonthlyPayment { get; init; }
    
    public BigDecimal Apr { get; init; }

    public List<AmortizationEntry> AmortizationSchedule { get; init; } = [];
}
