namespace FinancialCalculator.Common;

public static class CreditConstraints
{
    public static readonly int MinLoanTermInMonths = 1;
    public static readonly int MaxLoanTermInMonths = 960;
    public static readonly string ErrorLoanTermInMonths = "Loan month must be between 1 and 960.";
    
    public static readonly int MinPromoLoanTermInMonths = 0;
    public static readonly int MaxPromoLoanTermInMonths = 300;
    public static readonly string ErrorPromoLoanTermInMonths = "Promotional loan month must be between 0 and 300.";
    public static readonly string ErrorPromoLoanTermInMonthsTooHigh = "Promotional loan month must be lower than the loan term in months.";
    
    public static readonly int MinLoanAmount = 100;
    public static readonly string ErrorLoanAmount = "Loan amount must be at least 100 BGN.";
    
    public static readonly int MinAnnualInterestRateAmount = 0;
    public static readonly string ErrorAnnualInterestRateAmount = "Annual interest rate cannot be negative.";
    
    public static readonly int MinAnnualPromoInterestRateAmount = 0;
    public static readonly string ErrorAnnualPromoInterestRateAmount = "Annual promo interest rate cannot be negative.";
    
    public static readonly int MinGracePeriodMonths = 0;
    public static readonly int MaxGracePeriodMonths = 300;
    public static readonly string ErrorGracePeriodMonths = $"Grace period months have to be between 0 and 300.";
    
    public static readonly int MinPaymentsAmount = 1;
    public static readonly string ErrorPaymentsAmount = "Payments have to be at least 1.";
    
    public static readonly string ErrorPromotionalFields = "Both promotional fields need to be higher than 0 if used.";
}