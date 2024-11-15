using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService
{
    // LoanAmount should be:
    // - Greater than or equal to 100
    // - Less than or equal to 999,999,999
    public int LoanAmount { get; set; }
    public int LoanTermInMonths { get; set; }
    public decimal InterestRate { get; set; }
    public PaymentType PaymentType { get; set; }

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

        decimal applicationFeeValue = applicationFee ?? 0;
        decimal processingFeeValue = processingFee ?? 0;
        decimal otherFeesValue = otherFees ?? 0;

        decimal applicationFeeCalculated;
        if (applicationFeeType == FeeType.Percentage)
        {
            applicationFeeCalculated = ConvertPercentageToCurrency(loanAmount, applicationFeeValue);
        }
        else
        {
            applicationFeeCalculated = applicationFeeValue;
        }

        decimal processingFeeCalculated;
        if (processingFeeType == FeeType.Percentage)
        {
            processingFeeCalculated = CreditService.ConvertPercentageToCurrency(loanAmount, processingFeeValue);
        }
        else
        {
            processingFeeCalculated = processingFeeValue;
        }

        decimal otherFeesCalculated;
        if (otherFeesType == FeeType.Percentage)
        {
            otherFeesCalculated = CreditService.ConvertPercentageToCurrency(loanAmount, otherFeesValue);
        }
        else
        {
            otherFeesCalculated = otherFeesValue;
        }

        decimal totalInitialFees = applicationFeeCalculated + processingFeeCalculated + otherFeesCalculated;
        return totalInitialFees;
    }

    public static decimal ConvertPercentageToCurrency(int loanAmount, decimal percentage)
    {
        return loanAmount * (percentage / 100);
    }

    public static decimal CalculateAnnualManagementFee()
    {
        return 0;
    }

    public static decimal CalculateOtherAnnualFees()
    {
        return 0;
    }

    /////////////////////////////////////////////////////////////////////////////////////////

    public decimal CalculateAPR()
    {
        return 0;
    }

    public decimal CalculateMonthlyPayment()
    {
        return 0;
    }

    public decimal CalculateTotalInterestPaid()
    {
        return 0;
    }

    public decimal CalculateTotalPayments()
    {
        return 0;
    }

    public decimal CalculateTotalPaidWithInterestAndFees()
    {
        return 0;
    }
}
