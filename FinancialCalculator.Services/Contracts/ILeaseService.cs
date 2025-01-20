using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.Contracts;

public interface ILeaseService
{
    /// <summary>
    /// Calculates the lease result, including total payments.
    /// </summary>
    /// <param name="input">An instance of <see cref="LeaseServiceInputDto"/>.</param>
    /// <returns>A <see cref="LeaseResultDto"/> containing the calculated loan details.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any of the input parameters are outside the allowed ranges specified in <see cref="FinancialCalculator.Common.LeaseConstraints"/>.
    /// </exception>
    public LeaseResultDto CalculateLeaseResult(LeaseServiceInputDto input);
}
