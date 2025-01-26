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
        BigDecimal promoMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualPromotionalInterestRate, loanTermInMonths).BankersRounding(2);
        
        BigDecimal remainingBalance = this.CalculateRemainingBalance(loanAmount, annualPromotionalInterestRate, promotionalPeriodMonths, promoMonthlyPayment).BankersRounding(2);
        int remainingTerm = loanTermInMonths - promotionalPeriodMonths;
        BigDecimal normalMonthlyPayment = this.CalculateMonthlyPayment(remainingBalance, annualInterestRate, remainingTerm).BankersRounding(2);

        return new Tuple<BigDecimal, BigDecimal>(promoMonthlyPayment.BankersRounding(2), normalMonthlyPayment.BankersRounding(2));
    }
    
    /// <inheritdoc />
    public Tuple<BigDecimal, BigDecimal> CalculateMonthlyPaymentWithoutPromotional(
        BigDecimal loanAmount, 
        BigDecimal annualInterestRate, 
        int loanTermInMonths)
    {
        BigDecimal normalMonthlyPayment = this.CalculateMonthlyPayment(loanAmount, annualInterestRate, loanTermInMonths);
        return new Tuple<BigDecimal, BigDecimal>(BigDecimal.Zero, normalMonthlyPayment.BankersRounding(2));
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
        
        // Standard loan payment formula
        
        BigDecimal monthlyInterestRate = annualInterestRate / new BigDecimal(100) / new BigDecimal(12);

        BigDecimal num = BigDecimal.One + monthlyInterestRate;
        BigDecimal compoundInterestFactor = num.Power(new BigInteger(payments));
        BigDecimal numerator = loanAmount * monthlyInterestRate * compoundInterestFactor;
        BigDecimal denominator = compoundInterestFactor - BigDecimal.One;

        return (numerator / denominator).BankersRounding(4);
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
            BigDecimal interestPayment = (balance * monthlyInterestRate).BankersRounding(2);
            BigDecimal principalPayment = monthlyPayment - interestPayment;
            balance -= principalPayment;
        }

        return balance.BankersRounding(2);
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
        
        // Present Value of an Annuity formula
        // PV = Payment * 1 - (1 + interest)^-remaining / interest
        
        BigDecimal num = BigDecimal.One + monthlyInterestRate;
        BigDecimal positivePower = num.Power(new BigInteger(remainingMonths));
        BigDecimal fraction = BigDecimal.One / positivePower; 
        BigDecimal expression = BigDecimal.One - fraction;
        
        BigDecimal remainingPrincipal = payment * expression / monthlyInterestRate;

        return remainingPrincipal;
    }
}
