using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.DTO;

public class CreditInputDto
{
    public decimal LoanAmount { get; set; }
    
    public int LoanTermInMonths { get; set; }
    
    public int PromotionalPeriodMonths { get; set; }
    
    public double AnnualPromotionalInterestRate { get; set; }
    
    public double AnnualInterestRate { get; set; }

    public int GracePeriodMonths { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public decimal ApplicationFee { get; set; }
    public FeeType ApplicationFeeType { get; set; }
    
    public decimal ProcessingFee { get; set; }
    public FeeType ProcessingFeeType { get; set; }
    
    public decimal OtherInitialFees { get; set; }
    public FeeType OtherInitialFeesType { get; set; }
    
    public decimal MonthlyManagementFee { get; set; }
    public FeeType MonthlyManagementFeeType { get; set; }
    
    public decimal OtherMonthlyFees { get; set; }
    public FeeType OtherMonthlyFeesType { get; set; }
    
    public decimal AnnualManagementFee { get; set; }
    public FeeType AnnualManagementFeeType { get; set; }
    
    public decimal OtherAnnualFees { get; set; }
    public FeeType OtherAnnualFeesType { get; set; }
}
