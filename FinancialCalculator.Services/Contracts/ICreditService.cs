using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    /// <summary>
    /// Calculates the credit result, including total payments, interest, principal, and average monthly payment.
    /// </summary>
    /// <param name="serviceInput">An instance of <see cref="CreditServiceInputDto"/> containing all loan parameters and fees.</param>
    /// <returns>A <see cref="CreditResultDto"/> containing the calculated loan details.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any of the input parameters are outside the allowed ranges specified in <see cref="FinancialCalculator.Common.CreditConstraints"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when promotional period parameters are inconsistent or invalid.
    /// </exception>
    public CreditResultDto CalculateCreditResult(CreditServiceInputDto serviceInput);
}
