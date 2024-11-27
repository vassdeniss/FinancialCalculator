using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

using static FinancialCalculator.Common.CreditConstraints;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService, ICreditFeeService
{
    public decimal CalculateMonthlyPayment(decimal loanAmount, double annualInterestRate, int payments)
    {
        if (loanAmount < MinLoanAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(loanAmount), ErrorLoanAmount);
        }
        
        if (annualInterestRate < MinAnnualInterestRateAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(annualInterestRate), ErrorAnnualInterestRateAmount);
        }
        
        if (payments < MinPaymentsAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(payments), ErrorPaymentsAmount);
        }
        
        double monthlyInterestRate = annualInterestRate / 100 / 12;

        decimal compoundInterestFactor = (decimal)Math.Pow(1 + monthlyInterestRate, payments);
        decimal numerator = loanAmount * (decimal)monthlyInterestRate * compoundInterestFactor;
        decimal denominator = compoundInterestFactor - 1;

        return numerator / denominator;
    }

    public decimal CalculateRemainingBalance(decimal loanAmount, double annualInterestRate, int numberOfPayments,
        decimal monthlyPayment)
    {
        throw new NotImplementedException();
    }


    // ----- TODO: REFACTOR -----
    [Obsolete("Awaiting refactor.")]
    public decimal CalculateInitialFees(
        decimal loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType)
    {
        // All fields must be greater than zero if not null.
        if (applicationFee is <= 0 || processingFee is <= 0 || otherFees is <= 0)
        {
            throw new ArgumentException("All fees must be greater than zero if specified.");
        }

        // No single percentage field can be greater than 40%.
        if ((applicationFeeType == FeeType.Percentage && applicationFee is > 40) ||
            (processingFeeType == FeeType.Percentage && processingFee is > 40) ||
            (otherFeesType == FeeType.Percentage && otherFees is > 40))
        {
            throw new ArgumentException("Percentage-based fees cannot exceed 40%.");
        }

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

        // Ensure the total fees (both percentage converted to currency and currency) are less than half of the loan amount.
        if (totalInitialFees >= loanAmount / 2)
        {
            throw new ArgumentException("Total initial fees must be less than half of the loan amount.");
        }

        return totalInitialFees;
    }

    /// <inheritdoc />
    [Obsolete("Awaiting refactor.")]
    public decimal CalculateAnnualFees(
        decimal loanAmount,
        decimal? managementFee, FeeType managementFeeType,
        decimal? otherFees, FeeType otherFeesType,
        int loanTermInMonths)
    {
        if (loanTermInMonths <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loanTermInMonths), "Loan term must be greater than 0.");
        }
        
        // All fields must be greater than zero if not null.
        if (managementFee is <= 0 || otherFees is <= 0)
        {
            throw new ArgumentException("All fees must be greater than zero if specified.");
        }
        
        // No single percentage field can be greater than 40%.
        if ((managementFeeType == FeeType.Percentage && managementFee is > 40) ||
            (otherFeesType == FeeType.Percentage && otherFees is > 40))
        {
            throw new ArgumentException("Percentage-based fees cannot exceed 40%.");
        }
        
        // Treat as 0 if any fee value is null
        decimal managementFeeValue = managementFee ?? 0;
        decimal otherFeesValue = otherFees ?? 0;

        // If a percentage is specified, convert to currency, if not use value directly
        decimal managementFeeCalculated = managementFeeType == FeeType.Percentage
            ? ConvertPercentageToCurrency(loanAmount, managementFeeValue)
            : managementFeeValue;

        decimal otherFeesCalculated = otherFeesType == FeeType.Percentage
            ? ConvertPercentageToCurrency(loanAmount, otherFeesValue)
            : otherFeesValue;

        // Total annual fees over the loan term
        int totalYears = loanTermInMonths / 12;
        decimal totalAnnualFees = (managementFeeCalculated + otherFeesCalculated) * totalYears;

        // Ensure the total fees (both percentage converted to currency and currency) are less than half of the loan amount.
        if (totalAnnualFees >= loanAmount / 2)
        {
            throw new ArgumentException("Total annual fees must be less than half of the loan amount.");
        }

        return totalAnnualFees;
    }
    
    /// <inheritdoc />
    [Obsolete("Awaiting refactor.")]
    public decimal CalculateMonthlyFees(
        decimal loanAmount,
        decimal? managementFee, FeeType managementFeeType,
        decimal? otherFees, FeeType otherFeesType,
        int loanTermInMonths)
    {
        if (loanTermInMonths <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loanTermInMonths), "Loan term must be greater than 0.");
        }
        
        // All fields must be greater than zero if not null.
        if (managementFee is <= 0 || otherFees is <= 0)
        {
            throw new ArgumentException("All fees must be greater than zero if specified.");
        }
        
        // No single percentage field can be greater than 40%.
        if ((managementFeeType == FeeType.Percentage && managementFee is > 40) ||
            (otherFeesType == FeeType.Percentage && otherFees is > 40))
        {
            throw new ArgumentException("Percentage-based fees cannot exceed 40%.");
        }
        
        // Treat as 0 if any fee value is null
        decimal managementFeeValue = managementFee ?? 0;
        decimal otherFeesValue = otherFees ?? 0;

        // If a percentage is specified, convert to currency, if not use value directly
        decimal managementFeeCalculated = managementFeeType == FeeType.Percentage
            ? ConvertPercentageToCurrency(loanAmount, managementFeeValue)
            : managementFeeValue;

        decimal otherFeesCalculated = otherFeesType == FeeType.Percentage
            ? ConvertPercentageToCurrency(loanAmount, otherFeesValue)
            : otherFeesValue;
        
        decimal totalMonthlyFees = (managementFeeCalculated + otherFeesCalculated) * loanTermInMonths;

        // Ensure the total fees (both percentage converted to currency and currency) are less than half of the loan amount.
        if (totalMonthlyFees >= loanAmount / 2)
        {
            throw new ArgumentException("Total monthly fees must be less than half of the loan amount.");
        }

        return totalMonthlyFees;
    }
    
    /// <summary>
    /// Converts a percentage to a currency amount based on the loan amount.
    /// </summary>
    /// <param name="loanAmount">The loan principal.</param>
    /// <param name="percentage">The percentage value to convert.</param>
    /// <returns>The corresponding value in currency.</returns>
    [Obsolete("Awaiting refactor.")]
    private static decimal ConvertPercentageToCurrency(decimal loanAmount, decimal percentage)
    {
        return loanAmount * (percentage / 100);
    }
}
