using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    /// <summary>
    /// Gets or sets the total loan amount borrowed.
    /// </summary>
    int LoanAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the loan term in months.
    /// Represents the total number of months over which the loan will be repaid.
    /// </summary>
    int LoanTermInMonths { get; set; }
    
    /// <summary>
    /// Gets or sets the annual nominal interest rate as a decimal (e.g., 0.05 for 5%).
    /// </summary>
    decimal InterestRate { get; set; }
    
    /// <summary>
    /// Gets or sets the type of payment schedule.
    /// Determines whether the loan payments are annuity or declining balance.
    /// </summary>
    PaymentType PaymentType { get; set; }

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
    decimal CalculateInitialFees(
        decimal loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType);
    decimal CalculateAPR();
    decimal CalculateMonthlyPayment();
    decimal CalculateTotalInterestPaid();
    decimal CalculateTotalPayments();
    decimal CalculateTotalPaidWithInterestAndFees();
}
