using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class RefinanceResultDto
{
    public BigDecimal CurrentMonthlyInstallment { get; set; }
    public BigDecimal CurrentTotalPaid { get; set; }
    public BigDecimal CurrentEarlyRepaymentFee { get; set; }

    public BigDecimal NewMonthlyInstallment { get; set; }
    public BigDecimal NewTotalPaid { get; set; }
    public BigDecimal NewInitialFees { get; set; }

    public BigDecimal SavingsDifference { get; set; }

    public string Message { get; set; } = null!;
}
