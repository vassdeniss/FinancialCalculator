using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService
{
    // LoanAmount should be:
    // - Greater than or equal to 100
    // - Less than or equal to 999,999,999
    
    /// <inheritdoc />
    public int LoanAmount { get; set; }
    
    /// <inheritdoc />
    public int LoanTermInMonths { get; set; }
    
    /// <inheritdoc />
    public decimal InterestRate { get; set; }
    
    /// <inheritdoc />
    public PaymentType PaymentType { get; set; }

    /// <inheritdoc />
    public decimal CalculateInitialFees(
        int loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType)
    {
        // Validations required:
        // - All fields must be greater than zero if not null.
        // - No single percentage field (processingFeePercentage, applicationFeePercentage, otherFeePercentage) can be greater than 40%.
        // - Ensure the total fees (both percentage converted to currency and currency) are less than half of the loan amount.

        // Treat as 0 if any fee value is null
        decimal applicationFeeValue = applicationFee ?? 0;
        decimal processingFeeValue = processingFee ?? 0;
        decimal otherFeesValue = otherFees ?? 0;
        
        // If a percentage is specified, convert to currency, if not use value directly
        
        decimal applicationFeeCalculated = applicationFeeType == FeeType.Percentage 
            ? ConvertPercentageToCurrency(loanAmount, applicationFeeValue) 
            : applicationFeeValue;

        decimal processingFeeCalculated = processingFeeType == FeeType.Percentage 
            ? ConvertPercentageToCurrency(loanAmount, processingFeeValue) 
            : processingFeeValue;

        decimal otherFeesCalculated = otherFeesType == FeeType.Percentage 
            ? ConvertPercentageToCurrency(loanAmount, otherFeesValue) 
            : otherFeesValue;

        decimal totalInitialFees = applicationFeeCalculated + processingFeeCalculated + otherFeesCalculated;
        return totalInitialFees;
    }

    /// <inheritdoc />
    public static decimal CalculateAnnualManagementFee()
    {
        return 0;
    }

    /// <inheritdoc />
    public static decimal CalculateOtherAnnualFees()
    {
        return 0;
    }

    /////////////////////////////////////////////////////////////////////////////////////////

    /// <inheritdoc />
    public decimal CalculateAPR()
    {
        return 0;
    }

    /// <inheritdoc />
    public decimal CalculateMonthlyPayment()
    {
        return 0;
    }

    /// <inheritdoc />
    public decimal CalculateTotalInterestPaid()
    {
        return 0;
    }

    /// <inheritdoc />
    public decimal CalculateTotalPayments()
    {
        return 0;
    }

    /// <inheritdoc />
    public decimal CalculateTotalPaidWithInterestAndFees()
    {
        return 0;
    }
    
    /// <summary>
    /// Converts a percentage to a currency amount based on the loan amount.
    /// </summary>
    /// <param name="loanAmount">The loan principal.</param>
    /// <param name="percentage">The percentage value to convert.</param>
    /// <returns>The corresponding value in currency.</returns>
    private static decimal ConvertPercentageToCurrency(int loanAmount, decimal percentage)
    {
        return loanAmount * (percentage / 100);
    }
}
