using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.DTO;

public class CreditInputDto
{
    public BigDecimal LoanAmount { get; set; }
    
    public int LoanTermInMonths { get; set; }
    
    public int PromotionalPeriodMonths { get; set; }
    
    public BigDecimal AnnualPromotionalInterestRate { get; set; }
    
    public BigDecimal AnnualInterestRate { get; set; }

    public int GracePeriodMonths { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public BigDecimal ApplicationFee { get; set; }
    public FeeType ApplicationFeeType { get; set; }
    
    public BigDecimal ProcessingFee { get; set; }
    public FeeType ProcessingFeeType { get; set; }
    
    public BigDecimal OtherInitialFees { get; set; }
    public FeeType OtherInitialFeesType { get; set; }
    
    public BigDecimal MonthlyManagementFee { get; set; }
    public FeeType MonthlyManagementFeeType { get; set; }
    
    public BigDecimal OtherMonthlyFees { get; set; }
    public FeeType OtherMonthlyFeesType { get; set; }
    
    public BigDecimal AnnualManagementFee { get; set; }
    public FeeType AnnualManagementFeeType { get; set; }
    
    public BigDecimal OtherAnnualFees { get; set; }
    public FeeType OtherAnnualFeesType { get; set; }
}
