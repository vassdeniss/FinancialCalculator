using System.Numerics;
using FinancialCalculator.Common;
using FinancialCalculator.Services.Contracts;

namespace FinancialCalculator.Services;

public class PaymentCalculationService : IPaymentCalculationService
{
    /// <inheritdoc />
    public Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithPromotional(
        BigDecimal loanAmount, 
        int promotionalPeriodMonths, 
        BigDecimal annualPromotionalInterestRate, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths)
    {
        BigDecimal promoMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualPromotionalInterestRate, loanTermInMonths);
        
        BigDecimal remainingBalance = this.CalculateRemainingBalance(loanAmount, annualPromotionalInterestRate, promotionalPeriodMonths, promoMonthlyPayment);
        int remainingTerm = loanTermInMonths - promotionalPeriodMonths;
        BigDecimal normalMonthlyPayment = this.CalculateMonthlyPayment(remainingBalance, annualInterestRate, remainingTerm);

        return new Tuple<BigDecimal, BigDecimal>(promoMonthlyPayment, normalMonthlyPayment);
    }
    
    /// <inheritdoc />
    public Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithoutPromotional(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths)
    {
        BigDecimal normalMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualInterestRate, loanTermInMonths);
        return new Tuple<BigDecimal, BigDecimal>(BigDecimal.Zero, normalMonthlyPayment);
    }
    
    /// <inheritdoc />
    public BigDecimal CalculateMonthlyPayment(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int payments)
    {
        if (annualInterestRate == BigDecimal.Zero)
        {
            return loanAmount / new BigDecimal(payments);
        }
        
        BigDecimal monthlyInterestRate = annualInterestRate / new BigDecimal(100) / new BigDecimal(12);

        BigDecimal num = BigDecimal.One + monthlyInterestRate;
        BigDecimal compoundInterestFactor = num.Power(new BigInteger(payments));
        BigDecimal numerator = loanAmount * monthlyInterestRate * compoundInterestFactor;
        BigDecimal denominator = compoundInterestFactor - BigDecimal.One;

        return numerator / denominator;
    }

    /// <inheritdoc />
    public BigDecimal CalculateRemainingBalance(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int payments,
        BigDecimal monthlyPayment)
    {
        BigDecimal monthlyInterestRate = annualInterestRate / new BigDecimal(100) / new BigDecimal(12);
        
        BigDecimal balance = loanAmount;
        for (int i = 0; i < payments; i++)
        {
            BigDecimal interestPayment = balance * monthlyInterestRate;
            BigDecimal principalPayment = monthlyPayment - interestPayment;
            balance -= principalPayment.Round(2);
        }

        return balance;
    }
}
