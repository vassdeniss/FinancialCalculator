using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    /// <summary>
    /// Calculates the average monthly payment for a loan based on the specified parameters.
    /// </summary>
    /// <param name="interestRate">The annual interest rate for the loan, expressed as a decimal.</param>
    /// <param name="loanAmount">The loan principal amount.</param>
    /// <param name="loanTermInMonths">The loan term in months.</param>
    /// <param name="paymentType">The type of payment structure, either
    /// <see cref="PaymentType.Annuity"/> or <see cref="PaymentType.Decreasing"/>.</param>
    /// <returns>
    /// The average monthly payment, rounded to two decimal places.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description><paramref name="loanAmount"/> is less than 100 or greater than 999,999,999.</description> </item>
    /// <item><description><paramref name="interestRate"/> is less than 0 or greater than 9,999,999.</description> </item>
    /// <item><description><paramref name="loanTermInMonths"/> is less than 1 or greater than 960.</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>For annuity payments, <paramref name="loanAmount"/> is 101, <paramref name="loanTermInMonths"/>
    /// is 960, and <paramref name="interestRate"/> exceeds 1301.45.</description> </item>
    /// <item><description>For annuity payments, <paramref name="loanAmount"/> is 999,999,999,
    /// <paramref name="loanTermInMonths"/> is 960, and <paramref name="interestRate"/> exceeds 1259.82.</description> </item>
    /// </list>
    /// </exception>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="paymentType"/> is not supported.</exception>
    /// <remarks>
    /// For <see cref="PaymentType.Annuity"/>, the annuity formula is used to calculate the monthly payments.
    /// For <see cref="PaymentType.Decreasing"/>, payments decrease over time as the principal is repaid.
    /// If the monthly interest rate is 0, the average monthly payment is calculated as the loan amount divided by the loan term in months.
    /// </remarks>
    decimal CalculateAverageMonthlyPayment(decimal interestRate, decimal loanAmount, 
        int loanTermInMonths, PaymentType paymentType);
}
