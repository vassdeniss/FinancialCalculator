namespace FinancialCalculator.Common;

public class LeaseConstraints
{
    public const int MIN_PRICE = 100;
    public const int MAX_PRICE = 999999999;
    public const string ERROR_PRICE = "Price must be between 100 and 999999999.";

    public const int MIN_INITIAL_PAYMENT = 0;
    public const string ERROR_INITIAL_PAYMENT = "Initial payment must be a positive number.";
    public const string ERROR_INITIAL_PAYMENT_AGAINST_PRICE = "Initial payment cannot equal to the price or be greter.";
    
    public const int MIN_LEASE_TERM_IN_MONTHS = 1;
    public const int MAX_LEASE_TERM_IN_MONTHS = 120;
    public const string ERROR_LEASE_TERM_IN_MONTHS = "Lease month must be between 1 and 120.";
 
    public const int MIN_MONTHLY_INSTALLMENT = 1;
    public const int MAX_MONTHLY_INSTALLMENT = 999;
    public const string ERROR_MONTHLY_INSTALLMENT = "Monthly installment must be between 1 and 999.";
    public const string ERROR_MONTHLY_INSTALLMENT_AGAINST_PRICE = "Monthly installment must be less than the price.";
}
