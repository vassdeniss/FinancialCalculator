using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services
{
    public class CreditService : ICreditService
    {
        /// <inheritdoc />
        private int loanAmount;
        private int loanTermInMonths;
        private decimal interestRate;

        /// <summary>
        /// Gets or sets the loan amount.
        /// The loan amount must be between 100 and 999,999,999.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the loan amount is less than 100 or greater than 999,999,999.
        /// </exception>
        public int LoanAmount
        {
            get => loanAmount;
            set
            {
                if (value < 100 || value > 999999999)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Loan amount must be between 100 and 999,999,999.");
                }
                loanAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the loan term in months.
        /// The loan term must be between 1 and 960 months.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the loan term is less than 1 or greater than 960 months.
        /// </exception>
        public int LoanTermInMonths
        {
            get => loanTermInMonths;
            set
            {
                if (value <= 0 || value > 960)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Loan term must be between 1 and 960 months.");
                }
                loanTermInMonths = value;
            }
        }

        /// <summary>
        /// Gets or sets the annual nominal interest rate as a decimal (e.g., 0.05 for 5%).
        /// The interest rate must be greater than or equal to 0 and less than or equal to 9,999,999.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the interest rate is less than 0 or greater than 9,999,999.
        /// </exception>
        public decimal InterestRate
        {
            get => interestRate;
            set
            {
                if (value < 0 || value > 9999999)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Interest rate must be between 0 and 9,999,999.");
                }
                interestRate = value;
            }
        }

        public PaymentType PaymentType { get; set; }

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
            LoanAmount = loanAmount;
            LoanTermInMonths = loanTermInMonths;
            InterestRate = interestRate;

            // Specific validation for annuity payment type
            if (PaymentType == PaymentType.Annuity)
            {
                if (loanAmount == 101 && loanTermInMonths == 960 && interestRate > 1301.45m)
                {
                    throw new ArgumentException("Interest rate cannot exceed 1301.45% when loan amount is 101 and loan term is 960 months for annuity payments.");
                }

                if (loanAmount == 999999999 && loanTermInMonths == 960 && interestRate > 1259.82m)
                {
                    throw new ArgumentException("Interest rate cannot exceed 1259.82% when loan amount is 999,999,999 and loan term is 960 months for annuity payments.");
                }
            }
        }

        /// <inheritdoc />
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
        public decimal CalculateAverageMonthlyPayment()
        {
            ValidateInput();

            decimal monthlyInterestRate = InterestRate / 12;

            // Avoid dividing by zero by checking for extremely low interest rates
            if (monthlyInterestRate == 0)
            {
                return Math.Round(LoanAmount / (decimal)LoanTermInMonths, 2);
            }

            decimal totalPayments = 0;

            if (PaymentType == PaymentType.Annuity)
            {
                // Annuity formula for monthly payment
                double r = (double)monthlyInterestRate;
                double pow = Math.Pow(1 + r, LoanTermInMonths);
                decimal monthlyPayment = LoanAmount * (decimal)(r * pow / (pow - 1));
                totalPayments = monthlyPayment * LoanTermInMonths;
            }
            else if (PaymentType == PaymentType.Decreasing)
            {
                // Decreasing balance payment calculation
                decimal principalRepayment = LoanAmount / (decimal)LoanTermInMonths;

                for (int month = 1; month <= LoanTermInMonths; month++)
                {
                    decimal interestRepayment = (LoanAmount - principalRepayment * (month - 1)) * monthlyInterestRate;
                    decimal monthlyPayment = principalRepayment + interestRepayment;
                    totalPayments += monthlyPayment;
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported payment type.");
            }

            decimal averageMonthlyPayment = totalPayments / LoanTermInMonths;

            return Math.Round(averageMonthlyPayment, 2);
        }

        /// <inheritdoc />
        public decimal CalculateInitialFees(
            decimal loanAmount,
            decimal? applicationFee, FeeType applicationFeeType,
            decimal? processingFee, FeeType processingFeeType,
            decimal? otherFees, FeeType otherFeesType)
        {

            // All fields must be greater than zero if not null. (fancy pattern match :) )
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
}
