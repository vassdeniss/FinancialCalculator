using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class RefinanceResultDto
{
    public BigDecimal CurrentMonthlyInstallment { get; init; }
    public BigDecimal CurrentTotalPaid { get; init; }
    public BigDecimal CurrentEarlyRepaymentFee { get; init; }

    public BigDecimal NewMonthlyInstallment { get; init; }
    public BigDecimal NewTotalPaid { get; init; }
    public BigDecimal NewInitialFees { get; init; }

    public BigDecimal SavingsDifference { get; init; }

    public string Message { get; init; } = null!;
}
