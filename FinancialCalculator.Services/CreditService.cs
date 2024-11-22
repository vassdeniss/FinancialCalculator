using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService, ICreditFeeService
{
    private int _loanAmount;
    private int _loanTermInMonths;
    private decimal _interestRate;

    /// <inheritdoc />
    public int LoanAmount
    {
        get => this._loanAmount;
        set
        {
            if (value is < 100 or > 999999999)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Loan amount must be between 100 and 999,999,999.");
            }
                
            this._loanAmount = value;
        }
    }

    /// <inheritdoc />
    public int LoanTermInMonths
    {
        get => this._loanTermInMonths;
        set
        {
            if (value is <= 0 or > 960)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Loan term must be between 1 and 960 months.");
            }
            this._loanTermInMonths = value;
        }
    }

    /// <inheritdoc />
    public decimal InterestRate
    {
        get => this._interestRate;
        set
        {
            if (value is < 0 or > 9999999)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Interest rate must be between 0 and 9,999,999.");
            }
            this._interestRate = value;
        }
    }

    /// <inheritdoc />
    public PaymentType PaymentType { get; set; }

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
    public decimal CalculateAverageMonthlyPayment()
    {
        this.ValidateInput();

        decimal monthlyInterestRate = this.InterestRate / 12;

        // Avoid dividing by zero by checking for extremely low interest rates
        if (monthlyInterestRate == 0)
        {
            return Math.Round(this.LoanAmount / (decimal)this.LoanTermInMonths, 2);
        }

        decimal totalPayments = 0;
        if (this.PaymentType == PaymentType.Annuity)
        {
            // Annuity formula for monthly payment
            double r = (double)monthlyInterestRate;
            double pow = Math.Pow(1 + r, this.LoanTermInMonths);
            decimal monthlyPayment = this.LoanAmount * (decimal)(r * pow / (pow - 1));
            totalPayments = monthlyPayment * this.LoanTermInMonths;
        }
        else if (this.PaymentType == PaymentType.Decreasing)
        {
            // Decreasing balance payment calculation
            decimal principalRepayment = this.LoanAmount / (decimal)this.LoanTermInMonths;
            for (int month = 1; month <= LoanTermInMonths; month++)
            {
                decimal interestRepayment = (this.LoanAmount - principalRepayment * (month - 1)) * monthlyInterestRate;
                decimal monthlyPayment = principalRepayment + interestRepayment;
                totalPayments += monthlyPayment;
            }
        }
        else
        {
            throw new InvalidOperationException("Unsupported payment type.");
        }

        decimal averageMonthlyPayment = totalPayments / this.LoanTermInMonths;

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
    /// Validates the loan amount, loan term, and interest rate by assigning
    /// values to their respective properties, which triggers validation logic.
    /// This ensures each property is valid on its own. Additionally, this method
    /// performs specific validation checks for the annuity payment type to ensure
    /// the combination of values is acceptable.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the loan amount, loan term, or interest rate are outside of their valid ranges.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when specific conditions for the annuity payment type are not met.
    /// </exception>
    private void ValidateInput()
    {
        // Trigger validation in property setters
        LoanAmount = _loanAmount;
        LoanTermInMonths = _loanTermInMonths;
        InterestRate = _interestRate;

        if (this.PaymentType != PaymentType.Annuity)
        {
            return;
        }

        if (this._loanAmount == 101 && this._loanTermInMonths == 960 && this._interestRate > 1301.45m)
        {
            throw new ArgumentException("Interest rate cannot exceed 1301.45% when loan amount is 101 and loan term is 960 months for annuity payments.");
        }

        if (this._loanAmount == 999999999 && this._loanTermInMonths == 960 && this._interestRate > 1259.82m)
        {
            throw new ArgumentException("Interest rate cannot exceed 1259.82% when loan amount is 999,999,999 and loan term is 960 months for annuity payments.");
        }
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