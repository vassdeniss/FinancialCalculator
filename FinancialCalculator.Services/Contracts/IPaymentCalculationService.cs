using FinancialCalculator.Common;

namespace FinancialCalculator.Services.Contracts;

interface IPaymentCalculationService
{
    /// <summary>
    /// Calculates the monthly payments when a promotional period is present.
    /// </summary>
    /// <param name="loanAmount">The original loan amount.</param>
    /// <param name="promotionalPeriodMonths">The number of months in the promotional period.</param>
    /// <param name="annualPromotionalInterestRate">The annual interest rate during the promotional period (as a percentage).</param>
    /// <param name="annualInterestRate">The annual interest rate after the promotional period (as a percentage).</param>
    /// <param name="loanTermInMonths">The total loan term in months.</param>
    /// <returns>
    /// A <see cref="Tuple{BigDecimal, BigDecimal}"/> containing:
    /// <list type="bullet">
    /// <item><description>The promotional monthly payment.</description></item>
    /// <item><description>The normal monthly payment after the promotional period.</description></item>
    /// </list>
    /// </returns>
    Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithPromotional(
        BigDecimal loanAmount, 
        int promotionalPeriodMonths, 
        BigDecimal annualPromotionalInterestRate, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths);

    /// <summary>
    /// Calculates the monthly payment when there is no promotional period.
    /// </summary>
    /// <param name="loanAmount">The original loan amount.</param>
    /// <param name="annualInterestRate">The annual interest rate (as a percentage).</param>
    /// <param name="loanTermInMonths">The total loan term in months.</param>
    /// <returns>
    /// A <see cref="Tuple{BigDecimal, BigDecimal}"/> containing:
    /// <list type="bullet">
    /// <item><description>Zero as the promotional monthly payment.</description></item>
    /// <item><description>The normal monthly payment.</description></item>
    /// </list>
    /// </returns>
    Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithoutPromotional(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths);

    /// <summary>
    /// Calculates the monthly payment for a loan based on the loan amount, annual interest rate, and number of payments.
    /// </summary>
    /// <param name="loanAmount">
    /// The principal amount of the loan.
    /// </param>
    /// <param name="annualInterestRate">
    /// The annual interest rate as a percentage (e.g., 5 for 5%).
    /// </param>
    /// <param name="payments">
    /// The total number of monthly payments to be made.
    /// </param>
    /// <returns>
    /// The calculated monthly payment amount as a <c>BigDecimal</c>.
    /// </returns>
    /// <remarks>
    /// The formula used to calculate the monthly payment is based on the annuity formula:
    ///         P * r(1 + r)^n
    /// M = ----------------------
    ///         (1 + r)^n - 1
    /// Where:
    /// <list type="bullet">
    /// <item><description>M: Monthly payment.</description></item>
    /// <item><description>P: Loan amount (principal).</description></item>
    /// <item><description>r: Monthly interest rate (annual interest rate divided by 12).</description></item>
    /// <item><description>n: Total number of payments.</description></item>
    /// </list>
    /// </remarks>
    BigDecimal CalculateMonthlyPayment(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int payments);

    /// <summary>
    /// Calculates the remaining loan balance after a certain number of payments have been made.
    /// </summary>
    /// <param name="loanAmount">The original loan amount.</param>
    /// <param name="annualInterestRate">The annual interest rate (as a percentage).</param>
    /// <param name="payments">The number of payments made.</param>
    /// <param name="monthlyPayment">The monthly payment amount.</param>
    /// <returns>The remaining loan balance as a <c>BigDecimal</c>.</returns>
    BigDecimal CalculateRemainingBalance(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int payments,
        BigDecimal monthlyPayment);
}
