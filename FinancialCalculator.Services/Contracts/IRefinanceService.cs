using FinancialCalculator.Common;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.Contracts;

public interface IRefinanceService
{
    /// <summary>
    /// Main entry method that orchestrates the refinance comparison.
    /// </summary>
    /// <param name="input">A <see cref="RefinanceServiceInputDto"/> object containing all the
    /// needed parameters for the comparison (current loan, new loan, fees, etc.).
    /// </param>
    /// <returns>A <see cref="RefinanceResultDto"/> object containing the 
    /// monthly payment, total cost, fees, and a recommended message 
    /// about refinancing viability.
    /// </returns>
    public RefinanceResultDto Calculate(RefinanceServiceInputDto input);
}
