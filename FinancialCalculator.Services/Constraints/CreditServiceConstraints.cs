namespace FinancialCalculator.Services.Constraints;

[Obsolete("Awaiting refactor.")]
public static class CreditServiceConstraints
{
    public static readonly int MinGracePeriodMonths = 0;
    public static readonly int MaxGracePeriodMonths = 300;
    public static readonly string ErrorGracePeriodMonths = $"Grace period months have to be " +
                                                           $"between {MinGracePeriodMonths} and {MaxGracePeriodMonths}.";
}