using FinancialCalculator.Common;
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
    BigDecimal CalculateFee(BigDecimal feeValue, FeeType feeType, BigDecimal loanAmount);
    
    /// <summary>
    /// Calculates the new loan's initial fees, which are a combination
    /// of a percentage of the new principal plus a fixed currency fee.
    /// </summary>
    /// <param name="newLoanPrincipal">The principal amount for the new loan.</param>
    /// <param name="initialFeePercent">The percentage fee (e.g., 2% or 5%).</param>
    /// <param name="initialFeeCurrency">A fixed currency-based fee (e.g., 200 BGN).</param>
    /// <returns>The total of the initial fees for the new loan.</returns>
    BigDecimal CalculateInitialFees(BigDecimal newLoanPrincipal, BigDecimal initialFeePercent, BigDecimal initialFeeCurrency);
    
    /// <summary>
    /// Calculates the early repayment fee as a percentage of 
    /// the outstanding principal at the moment of repayment.
    /// </summary>
    /// <param name="outstandingPrincipal">The outstanding (remaining) principal at the time of early repayment.</param>
    /// <param name="earlyRepaymentFeePercent">The early repayment fee in percent.</param>
    /// <returns>The early repayment fee amount.</returns>
    BigDecimal CalculateEarlyRepaymentFee(BigDecimal outstandingPrincipal, BigDecimal earlyRepaymentFeePercent);
}
