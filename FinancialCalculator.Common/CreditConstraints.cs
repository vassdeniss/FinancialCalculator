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
    public const int MAX_LOAN_AMOUNT = 999999999;
    public const string ERROR_LOAN_AMOUNT = "Loan amount must be between 1 and 999999999 BGN.";
    
    public const int MIN_ANNUAL_INTEREST_RATE_AMOUNT = 0;
    public const int MAX_ANNUAL_INTEREST_RATE_AMOUNT = 9999999;
    public const string ERROR_ANNUAL_INTEREST_RATE_AMOUNT = "Annual interest rate must be between 1 and 9999999%.";
    
    public const int MIN_ANNUAL_PROMO_INTEREST_RATE_AMOUNT = 0;
    public const int MAX_ANNUAL_PROMO_INTEREST_RATE_AMOUNT = 40;
    public const string ERROR_ANNUAL_PROMO_INTEREST_RATE_AMOUNT = "Annual promo interest rate must be between 1 and 40%.";
    
    public const int MIN_GRACE_PERIOD_MONTHS = 0;
    public const int MAX_GRACE_PERIOD_MONTHS = 300;
    public const string ERROR_GRACE_PERIOD_MONTHS = "Grace period months have to be between 0 and 300.";
    
    public const string ERROR_PROMOTIONAL_FIELDS = "Both promotional fields need to be higher than 0 if used.";

    public const int MIN_APPLICATION_FEE = 0;
    public const long MAX_APPLICATION_FEE = 999999999999;
    public const string ERROR_APPLICATION_FEE = "Application fee must to be between 0 and 999999999999 if used.";
    
    public const int MIN_PROCESSING_FEE = 0;
    public const long MAX_PROCESSING_FEE = 999999999999;
    public const string ERROR_PROCESSING_FEE = "Processing fee must be between 0 and 999999999999 if used.";

    public const int MIN_OTHER_INITIAL_FEES = 0;
    public const long MAX_OTHER_INITIAL_FEES = 999999999999;
    public const string ERROR_OTHER_INITIAL_FEES = "Other initial fees must be between 0 and 999999999999 if used.";

    public const int MIN_MONTHLY_MANAGEMENT_FEE = 0;
    public const long MAX_MONTHLY_MANAGEMENT_FEE = 999999999999;
    public const string ERROR_MONTHLY_MANAGEMENT_FEE = "Monthly management fee must be between 0 and 999999999999 if used.";

    public const int MIN_OTHER_MONTHLY_FEES = 0;
    public const long MAX_OTHER_MONTHLY_FEES = 999999999999;
    public const string ERROR_OTHER_MONTHLY_FEES = "Other monthly fees must be between 0 and 999999999999 if used.";

    public const int MIN_ANNUAL_MANAGEMENT_FEE = 0;
    public const long MAX_ANNUAL_MANAGEMENT_FEE = 999999999999;
    public const string ERROR_ANNUAL_MANAGEMENT_FEE = "Annual management fee must be between 0 and 999999999999 if used.";

    public const int MIN_OTHER_ANNUAL_FEES = 0;
    public const long MAX_OTHER_ANNUAL_FEES = 999999999999;
    public const string ERROR_OTHER_ANNUAL_FEES = "Other annual fees must be between 0 and 999999999999 if used.";
}
