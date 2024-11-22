using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    /// <summary>
    /// Gets or sets the loan amount.
    /// The loan amount must be between 100 and 999,999,999.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the loan amount is less than 100 or greater than 999,999,999.
    /// </exception>
    int LoanAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the loan term in months.
    /// The loan term must be between 1 and 960 months.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the loan term is less than 1 or greater than 960 months.
    /// </exception>
    int LoanTermInMonths { get; set; }
    
    /// <summary>
    /// Gets or sets the annual nominal interest rate as a decimal (e.g., 0.05 for 5%).
    /// The interest rate must be greater than or equal to 0 and less than or equal to 9,999,999.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the interest rate is less than 0 or greater than 9,999,999.
    /// </exception>
    decimal InterestRate { get; set; }
    
    /// <summary>
    /// Gets or sets the type of payment schedule.
    /// Determines whether the loan payments are annuity or declining balance.
    /// </summary>
    PaymentType PaymentType { get; set; }
    
    /// <summary>
    /// Calculates the average monthly payment for a loan based on the payment type (Annuity or Decreasing).
    /// This method first validates the input values, then calculates the total payments depending on the
    /// payment type, and finally returns the average monthly payment rounded to two decimal places.
    /// </summary>
    /// <returns>
    /// The average monthly payment for the loan, rounded to two decimal places.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an unsupported payment type is specified.
    /// </exception>
    decimal CalculateAverageMonthlyPayment();
}
