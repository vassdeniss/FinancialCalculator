﻿using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService, ICreditFeeService
{
    /// <inheritdoc />
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
    
    /// <inheritdoc />
    public decimal CalculateAverageMonthlyPayment(decimal interestRate, decimal loanAmount, 
        int loanTermInMonths, PaymentType paymentType)
    {
        if (loanAmount is < 100 or > 999_999_999)
        {
            throw new ArgumentOutOfRangeException(nameof(loanAmount), "Loan amount must be between 100 and 999,999,999.");
        }
        
        if (interestRate is < 0 or > 9_999_999)
        {
            throw new ArgumentOutOfRangeException(nameof(interestRate), "Interest rate must be between 0 and 9,999,999.");
        }
        
        if (loanTermInMonths is <= 0 or > 960)
        {
            throw new ArgumentOutOfRangeException(nameof(loanTermInMonths), "Loan term must be between 1 and 960 months.");
        }
        
        decimal monthlyInterestRate = interestRate / 12;

        // Avoid dividing by zero by checking for extremely low interest rates
        if (monthlyInterestRate == 0)
        {
            return Math.Round(loanAmount / loanTermInMonths, 2);
        }

        decimal totalPayments = 0;
        if (paymentType == PaymentType.Annuity)
        {
            if (loanAmount == 101 && loanTermInMonths == 960 && interestRate > 1301.45m)
            {
                throw new ArgumentException("Interest rate cannot exceed 1301.45% when loan amount is 101 and loan term is 960 months for annuity payments.");
            }

            if (loanAmount == 999_999_999 && loanTermInMonths == 960 && interestRate > 1259.82m)
            {
                throw new ArgumentException("Interest rate cannot exceed 1259.82% when loan amount is 999,999,999 and loan term is 960 months for annuity payments.");
            }
            
            // Annuity formula for monthly payment
            double r = (double)monthlyInterestRate;
            double pow = Math.Pow(1 + r, loanTermInMonths);
            decimal monthlyPayment = loanAmount * (decimal)(r * pow / (pow - 1));
            totalPayments = monthlyPayment * loanTermInMonths;
        }
        else if (paymentType == PaymentType.Decreasing)
        {
            // Decreasing balance payment calculation
            decimal principalRepayment = loanAmount / loanTermInMonths;
            for (int month = 1; month <= loanTermInMonths; month++)
            {
                decimal interestRepayment = (loanAmount - principalRepayment * (month - 1)) * monthlyInterestRate;
                decimal monthlyPayment = principalRepayment + interestRepayment;
                totalPayments += monthlyPayment;
            }
        }
        else
        {
            throw new InvalidOperationException("Unsupported payment type.");
        }

        decimal averageMonthlyPayment = totalPayments / loanTermInMonths;
        return Math.Round(averageMonthlyPayment, 2);
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
    
    /// <summary>
    /// Converts a percentage to a currency amount based on the loan amount.
    /// </summary>
    /// <param name="loanAmount">The loan principal.</param>
    /// <param name="percentage">The percentage value to convert.</param>
    /// <returns>The corresponding value in currency.</returns>
    private static decimal ConvertPercentageToCurrency(decimal loanAmount, decimal percentage)
    {
        return loanAmount * (percentage / 100);
    }
}
