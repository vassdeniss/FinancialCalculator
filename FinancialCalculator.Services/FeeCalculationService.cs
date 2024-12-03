using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.Contracts;

namespace FinancialCalculator.Services;

class FeeCalculationService : IFeeCalculationService
{
    /// <inheritdoc />
    public decimal CalculateFee(decimal feeValue, FeeType feeType, decimal loanAmount)
    {
        return feeType == FeeType.Percentage
            ? loanAmount * (feeValue / 100m)
            : feeValue;
    }
}
