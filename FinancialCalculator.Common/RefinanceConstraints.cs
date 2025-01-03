namespace FinancialCalculator.Common;

public static class RefinanceConstraints
{
    public const int MIN_LOAN_TERM_IN_MONTHS = 1;
    public const int MAX_LOAN_TERM_IN_MONTHS = 999;
    public const string ERROR_LOAN_TERM_IN_MONTHS = "Loan month must be between 1 and 999.";
    
    public const int MIN_LOAN_AMOUNT = 1;
    public const int MAX_LOAN_AMOUNT = 999999999;
    public const string ERROR_LOAN_AMOUNT = "Loan amount must be between 1 and 999999999 BGN.";
    
    public const int MIN_ANNUAL_INTEREST_RATE_AMOUNT = 1;
    public const int MAX_ANNUAL_INTEREST_RATE_AMOUNT = 99;
    public const string ERROR_ANNUAL_INTEREST_RATE_AMOUNT = "Annual interest rate must be between 1 and 99%.";

    public const int MIN_CONTRIBUTIONS_MADE = 1;
    public const string ERROR_CONTRIBUTIONS_MADE = "Contibutions must be a positive number.";
    public const string ERROR_CONTRIBUTIONS_MADE_TOO_HIGH = "Contibutions must be lower than the loan term in months.";
    
    public const int MIN_EARLY_REPAYMENT_FEE = 0;
    public const int MAX_EARLY_REPAYMENT_FEE = 99;
    public const string ERROR_EARLY_REPAYMENT_FEE = "Early repayment fee must be between 0 and 99%.";
    
    public const int MIN_INITIAL_FEE = 0;
    public const int MAX_INITIAL_FEE = 999999999;
    public const string ERROR_INITIAL_FEE = "Initial fee must be between 0 and 999999999.";
}
