using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    /// <summary>
    /// Calculates the monthly payment for a loan based on the loan amount, annual interest rate, and number of payments.
    /// </summary>
    /// <param name="loanAmount">
    /// The principal amount of the loan. This must be greater than or equal
    /// to <see cref="FinancialCalculator.Common.CreditConstraints.MinLoanAmount"/>.
    /// </param>
    /// <param name="annualInterestRate">
    /// The annual interest rate as a percentage (e.g., 5 for 5%). This must be greater
    /// than or equal to <see cref="FinancialCalculator.Common.CreditConstraints.MinAnnualInterestRateAmount"/>.
    /// </param>
    /// <param name="payments">
    /// The total number of monthly payments to be made. This must be greater than or
    /// equal to <see cref="FinancialCalculator.Common.CreditConstraints.MinPaymentsAmount"/>.
    /// </param>
    /// <returns>
    /// The calculated monthly payment amount as a <c>decimal</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description><paramref name="loanAmount"/> is less than <see cref="FinancialCalculator.Common.CreditConstraints.MinLoanAmount"/>.</description></item>
    /// <item><description><paramref name="annualInterestRate"/> is less than <see cref="FinancialCalculator.Common.CreditConstraints.MinAnnualInterestRateAmount"/>.</description></item>
    /// <item><description><paramref name="payments"/> is less than <see cref="FinancialCalculator.Common.CreditConstraints.MinPaymentsAmount"/>.</description></item>
    /// </list>
    /// </exception>
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
    decimal CalculateMonthlyPayment(decimal loanAmount, double annualInterestRate, int payments);

    // TODO
    decimal CalculateRemainingBalance(decimal loanAmount, double annualInterestRate,
        int numberOfPayments, decimal monthlyPayment);
}
