using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditFeeService
{
    /// <summary>
    /// Calculates the total initial fees for the loan based on specified fees and types.
    /// </summary>
    /// <param name="loanAmount">The principal loan amount.</param>
    /// <param name="applicationFee">Optional application fee.</param>
    /// <param name="applicationFeeType">Specifies if the application fee is fixed or percentage-based.</param>
    /// <param name="processingFee">Optional processing fee.</param>
    /// <param name="processingFeeType">Specifies if the processing fee is fixed or percentage-based.</param>
    /// <param name="otherFees">Optional other fees.</param>
    /// <param name="otherFeesType">Specifies if the other fees are fixed or percentage-based.</param>
    /// <returns>The total initial fees calculated.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description><paramref name="applicationFee"/> is less than or equal to 0 if not null.</description> </item>
    /// <item><description><paramref name="processingFee"/> is less than or equal to 0 if not null.</description> </item>
    /// <item><description><paramref name="otherFees"/> is less than or equal to 0 if not null.</description></item>
    /// <item><description><paramref name="applicationFee"/> exceeds 40% if <paramref name="applicationFeeType"/> is percentage.</description></item>
    /// <item><description><paramref name="processingFee"/> exceeds 40% if <paramref name="processingFeeType"/> is percentage.</description></item>
    /// <item><description><paramref name="otherFees"/> exceeds 40% if <paramref name="otherFeesType"/> is percentage.</description></item>
    /// <item><description>Total fees are less than half of the loan amount.</description></item>
    /// </list>
    /// </exception>
    decimal CalculateInitialFees(
        decimal loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType);
    
    /// <summary>
    /// Calculates the annual fees for the loan based on specified fees and types.
    /// </summary>
    /// <param name="loanAmount">The loan principal amount.</param>
    /// <param name="managementFee">Optional management fee.</param>
    /// <param name="managementFeeType">Specifies if the management fee is fixed or percentage-based.</param>
    /// <param name="otherFees">Optional other fees.</param>
    /// <param name="otherFeesType">Specifies if the other fees are fixed or percentage-based.</param>
    /// <param name="loanTermInMonths">The loan term in months.</param>
    /// <returns>The annual fees for the loan term.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description><paramref name="managementFee"/> is less than or equal to 0 if not null.</description> </item>
    /// <item><description><paramref name="otherFees"/> is less than or equal to 0 if not null.</description></item>
    /// <item><description><paramref name="managementFee"/> exceeds 40% if <paramref name="managementFeeType"/> is percentage.</description></item>
    /// <item><description><paramref name="otherFees"/> exceeds 40% if <paramref name="otherFeesType"/> is percentage.</description></item>
    /// <item><description>The annual fees are less than half of the loan amount.</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="loanTermInMonths"/> is less than or equal to 0.
    /// </exception>
    public decimal CalculateAnnualFees(
        decimal loanAmount,
        decimal? managementFee, FeeType managementFeeType,
        decimal? otherFees, FeeType otherFeesType,
        int loanTermInMonths);
    
    /// <summary>
    /// Calculates the monthly fees for the loan based on specified fees and types.
    /// </summary>
    /// <param name="loanAmount">The loan principal amount.</param>
    /// <param name="managementFee">Optional management fee.</param>
    /// <param name="managementFeeType">Specifies if the management fee is fixed or percentage-based.</param>
    /// <param name="otherFees">Optional other fees.</param>
    /// <param name="otherFeesType">Specifies if the other fees are fixed or percentage-based.</param>
    /// <param name="loanTermInMonths">The loan term in months.</param>
    /// <returns>The monthly fees for the loan term.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description><paramref name="managementFee"/> is less than or equal to 0 if not null.</description> </item>
    /// <item><description><paramref name="otherFees"/> is less than or equal to 0 if not null.</description></item>
    /// <item><description><paramref name="managementFee"/> exceeds 40% if <paramref name="managementFeeType"/> is percentage.</description></item>
    /// <item><description><paramref name="otherFees"/> exceeds 40% if <paramref name="otherFeesType"/> is percentage.</description></item>
    /// <item><description>The monthly fees are less than half of the loan amount.</description></item>
    /// </list>
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="loanTermInMonths"/> is less than or equal to 0.
    /// </exception>
    public decimal CalculateMonthlyFees(
        decimal loanAmount,
        decimal? managementFee, FeeType managementFeeType,
        decimal? otherFees, FeeType otherFeesType,
        int loanTermInMonths);
}
