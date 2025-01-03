using FinancialCalculator.Common;

namespace FinancialCalculator.Web.Models;

public class RefinanceResult
{
    public BigDecimal CurrentAnnualInterestRate { get; init; }
    public BigDecimal NewAnnualInterestRate { get; init; }
    
    public int LoanTerm { get; init; }
    public int NewLoanTerm { get; init; }
    
    public BigDecimal CurrentMonthlyInstallment { get; init; }
    public BigDecimal CurrentTotalPaid { get; init; }
    public BigDecimal CurrentEarlyRepaymentFee { get; init; }

    public BigDecimal NewMonthlyInstallment { get; init; }
    public BigDecimal NewTotalPaid { get; init; }
    public BigDecimal NewInitialFees { get; init; }

    public BigDecimal SavingsDifference { get; init; }

    public string Message { get; init; } = null!;
}
