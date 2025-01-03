using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class RefinanceServiceInputDto
{
    public BigDecimal LoanAmount { get; init; }
    
    public int LoanTermInMonths { get; init; }
    
    public BigDecimal AnnualInterestRate { get; init; }
    
    public int ContributionsMade { get; init; }
    
    public BigDecimal EarlyRepaymentFee { get; init; }
    
    public BigDecimal NewAnnualInterestRate { get; init; }
    
    public BigDecimal InitialFee { get; init; }
    
    public BigDecimal InitialFeeCurrency { get; init; }
}
