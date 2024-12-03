using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.Contracts;

interface IFeeCalculationService
{
    /// <summary>
    /// Calculates the fee amount based on the fee value, fee type, and loan amount.
    /// </summary>
    /// <param name="feeValue">The value of the fee.</param>
    /// <param name="feeType">The type of the fee (Amount or Percentage).</param>
    /// <param name="loanAmount">The original loan amount.</param>
    /// <returns>The calculated fee amount as a <c>decimal</c>.</returns>
    decimal CalculateFee(decimal feeValue, FeeType feeType, decimal loanAmount);
}
