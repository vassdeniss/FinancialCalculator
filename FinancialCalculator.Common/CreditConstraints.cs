namespace FinancialCalculator.Common;

public static class CreditConstraints
{
    public static readonly int MinLoanAmount = 100;
    public static readonly string ErrorLoanAmount = "Loan amount must be at least 100 BGN.";
    
    public static readonly int MinAnnualInterestRateAmount = 0;
    public static readonly string ErrorAnnualInterestRateAmount = "Annual interest rate cannot be negative.";
    
    public static readonly int MinPaymentsAmount = 1;
    public static readonly string ErrorPaymentsAmount = "Payments have to be at least 1.";
}