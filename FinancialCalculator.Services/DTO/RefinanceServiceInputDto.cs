using FinancialCalculator.Common;

namespace FinancialCalculator.Services.DTO;

public class RefinanceServiceInputDto
{
    public BigDecimal LoanAmount { get; set; }
    
    public int LoanTermInMonths { get; set; }
    
    public BigDecimal AnnualInterestRate { get; set; }
    
    public int ContributionsMade { get; set; }
    
    public BigDecimal EarlyRepaymentFee { get; set; }
    
    public BigDecimal NewAnnualInterestRate { get; set; }
    
    public BigDecimal InitialFee { get; set; }
    
    public BigDecimal InitialFeeCurrency { get; set; }
}
