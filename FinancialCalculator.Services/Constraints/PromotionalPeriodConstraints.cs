namespace FinancialCalculator.Services.Constraints;

[Obsolete("Awaiting refactor.")]
public static class PromotionalPeriodConstraints
{
    public static readonly int MinGracePeriodMonths = 0;
    public static readonly int MaxGracePeriodMonths = 300;
    public static readonly string ErrorGracePeriodMonths = $"Grace period months have to be " +
                                                           $"between {MinGracePeriodMonths} and {MaxGracePeriodMonths}.";
    
    public static readonly int MinPromotionalRatePercentage = 0;
    public static readonly string ErrorPromotionalRatePercentage = $"Promotional rate percentage cannot be lower than " +
                                                                 $"{MinPromotionalRatePercentage}.";
    
    public static readonly int MinPromotionalPeriodMonths = 0;
    public static readonly int MaxPromotionalPeriodMonths = 300;
    public static readonly string ErrorPromotionalPeriodMonths = $"Promotional period months have to be " +
                                                           $"between {MinPromotionalPeriodMonths} and {MaxPromotionalPeriodMonths}.";
    
    public static readonly string ErrorPromotionalPeriod = "Promotional period and rate percentage have to be both not zero.";
    public static readonly string ErrorPromotionalPeriodLoanTerms = "Promotional and grace period cannot exceed loan months.";
}