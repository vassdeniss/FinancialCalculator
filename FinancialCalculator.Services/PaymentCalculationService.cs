using FinancialCalculator.Services.Contracts;

namespace FinancialCalculator.Services;

class PaymentCalculationService : IPaymentCalculationService
{
    /// <inheritdoc />
    public Tuple<decimal, decimal> CalculateMonthlyPaymentWithPromotional(
        decimal loanAmount, 
        int promotionalPeriodMonths, 
        double annualPromotionalInterestRate, 
        double annualInterestRate, 
        int loanTermInMonths)
    {
        decimal promoMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualPromotionalInterestRate, loanTermInMonths);
        
        decimal remainingBalance = this.CalculateRemainingBalance(loanAmount, annualPromotionalInterestRate, promotionalPeriodMonths, promoMonthlyPayment);
        int remainingTerm = loanTermInMonths - promotionalPeriodMonths;
        decimal normalMonthlyPayment = this.CalculateMonthlyPayment(remainingBalance, annualInterestRate, remainingTerm);

        return new Tuple<decimal, decimal>(promoMonthlyPayment, normalMonthlyPayment);
    }
    
    /// <inheritdoc />
    public Tuple<decimal, decimal> CalculateMonthlyPaymentWithoutPromotional(
        decimal loanAmount, 
        double annualInterestRate, 
        int loanTermInMonths)
    {
        decimal normalMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualInterestRate, loanTermInMonths);
        return new Tuple<decimal, decimal>(0.0m, normalMonthlyPayment);
    }
    
    /// <inheritdoc />
    public decimal CalculateMonthlyPayment(
        decimal loanAmount, 
        double annualInterestRate, 
        int payments)
    {
        double monthlyInterestRate = annualInterestRate / 100 / 12;

        decimal compoundInterestFactor = (decimal)Math.Pow(1 + monthlyInterestRate, payments);
        decimal numerator = loanAmount * (decimal)monthlyInterestRate * compoundInterestFactor;
        decimal denominator = compoundInterestFactor - 1;

        return numerator / denominator;
    }

    /// <inheritdoc />
    public decimal CalculateRemainingBalance(
        decimal loanAmount, 
        double annualInterestRate, 
        int payments,
        decimal monthlyPayment)
    {
        decimal monthlyInterestRate = (decimal)(annualInterestRate / 100 / 12);
        
        decimal balance = loanAmount;
        for (int i = 0; i < payments; i++)
        {
            decimal interestPayment = balance * monthlyInterestRate;
            decimal principalPayment = monthlyPayment - interestPayment;
            balance -= principalPayment;
        }

        return balance;
    }
}
