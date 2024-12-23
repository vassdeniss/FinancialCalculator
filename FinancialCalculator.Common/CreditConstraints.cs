namespace FinancialCalculator.Common;

public static class CreditConstraints
{
    public const int MIN_LOAN_TERM_IN_MONTHS = 1;
    public const int MAX_LOAN_TERM_IN_MONTHS = 960;
    public const string ERROR_LOAN_TERM_IN_MONTHS = "Loan month must be between 1 and 960.";
    
    public const int MIN_PROMO_LOAN_TERM_IN_MONTHS = 0;
    public const int MAX_PROMO_LOAN_TERM_IN_MONTHS = 300;
    public const string ERROR_PROMO_LOAN_TERM_IN_MONTHS = "Promotional loan month must be between 0 and 300.";
    public const string ERROR_PROMO_LOAN_TERM_IN_MONTHS_TOO_HIGH = "Promotional loan month must be lower than the loan term in months.";
    
    public const int MIN_LOAN_AMOUNT = 100;
    public const string ERROR_LOAN_AMOUNT = "Loan amount must be at least 100 BGN.";
    
    public const int MIN_ANNUAL_INTEREST_RATE_AMOUNT = 0;
    public const string ERROR_ANNUAL_INTEREST_RATE_AMOUNT = "Annual interest rate cannot be negative.";
    
    public const int MIN_ANNUAL_PROMO_INTEREST_RATE_AMOUNT = 0;
    public const string ERROR_ANNUAL_PROMO_INTEREST_RATE_AMOUNT = "Annual promo interest rate cannot be negative.";
    
    public const int MIN_GRACE_PERIOD_MONTHS = 0;
    public const int MAX_GRACE_PERIOD_MONTHS = 300;
    public const string ERROR_GRACE_PERIOD_MONTHS = $"Grace period months have to be between 0 and 300.";
    
    public const string ERROR_PROMOTIONAL_FIELDS = "Both promotional fields need to be higher than 0 if used.";
}
