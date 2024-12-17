using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.Contracts;

namespace FinancialCalculator.Services;

class FeeCalculationService : IFeeCalculationService
{
    /// <inheritdoc />
    public BigDecimal CalculateFee(BigDecimal feeValue, FeeType feeType, BigDecimal loanAmount)
    {
        return feeType == FeeType.Percentage
            ? loanAmount * feeValue / new BigDecimal(100)
            : feeValue;
    }
}
