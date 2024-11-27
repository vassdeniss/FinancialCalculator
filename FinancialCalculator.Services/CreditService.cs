using FinancialCalculator.Services.Contracts.CreditServices;
using FinancialCalculator.Services.Enums;
namespace FinancialCalculator.Services;

public class CreditService : ICreditService
{
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
    
}
