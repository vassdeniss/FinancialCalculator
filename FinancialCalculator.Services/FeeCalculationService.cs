using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.Contracts;

namespace FinancialCalculator.Services;

public class FeeCalculationService : IFeeCalculationService
{
    /// <inheritdoc />
    public BigDecimal CalculateFee(BigDecimal feeValue, FeeType feeType, BigDecimal loanAmount)
    {
        return feeType == FeeType.Percentage
            ? loanAmount * feeValue / new BigDecimal(100)
            : feeValue;
    }
    
    /// <inheritdoc />
    public BigDecimal CalculateInitialFees(BigDecimal newLoanPrincipal, BigDecimal initialFeePercent, BigDecimal initialFeeCurrency)
    {
        BigDecimal percentFee = newLoanPrincipal * (initialFeePercent / new BigDecimal(100));
        return percentFee + initialFeeCurrency;
    }
    
    /// <inheritdoc />
    public BigDecimal CalculateEarlyRepaymentFee(BigDecimal outstandingPrincipal, BigDecimal earlyRepaymentFeePercent)
    {
        return outstandingPrincipal * (earlyRepaymentFeePercent / new BigDecimal(100));
    }
}
