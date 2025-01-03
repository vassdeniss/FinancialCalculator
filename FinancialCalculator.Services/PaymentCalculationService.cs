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

        return new Tuple<BigDecimal, BigDecimal>(promoMonthlyPayment.Round(2), normalMonthlyPayment.Round(2));
    }
    
    /// <inheritdoc />
    public Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithoutPromotional(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths)
    {
        BigDecimal normalMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualInterestRate, loanTermInMonths);
        return new Tuple<BigDecimal, BigDecimal>(BigDecimal.Zero, normalMonthlyPayment.Round(2));
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
    
    /// <inheritdoc />
    public BigDecimal CalculateRemainingRefinanceBalance(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int payments,
        int paymentsMade)
    {
        // Fully paid
        if (paymentsMade >= payments)
        {
            return BigDecimal.Zero;
        }
        
        // Get the full monthly payment for the entire original term
        BigDecimal payment = this.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments);
        
        BigDecimal monthlyInterestRate = annualInterestRate / new BigDecimal(100) / new BigDecimal(12);
        int remainingMonths = payments - paymentsMade;
        
        // Remaining principal formula
        //   = Payment * (1 - (1 + i)^(-remainingMonths)) / i
        if (monthlyInterestRate == BigDecimal.Zero)
        {
            // If there's no interest, remaining principal is simply 
            // the fraction of principal not yet covered 
            // (assuming equal monthly payments).
            BigDecimal paidSoFar = payment * new BigDecimal(paymentsMade);
            BigDecimal leftover = loanAmount - paidSoFar;
            return leftover < BigDecimal.Zero ? BigDecimal.Zero : leftover;
        }
        
        BigDecimal num = BigDecimal.One + monthlyInterestRate;
        BigDecimal positivePower = num.Power(new BigInteger(remainingMonths));
        BigDecimal fraction = BigDecimal.One / positivePower; 
        BigDecimal expression = BigDecimal.One - fraction;
        
        BigDecimal remainingPrincipal = payment * expression / monthlyInterestRate;

        return remainingPrincipal;
    }
}
