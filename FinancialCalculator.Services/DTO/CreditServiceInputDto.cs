using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.DTO;

public class CreditServiceInputDto
{
    public CreditServiceInputDto(BigDecimal loanAmount, int loanTermInMonths, int promotionalPeriodMonths,
        BigDecimal annualPromotionalInterestRate, BigDecimal annualInterestRate, int gracePeriodMonths,
        PaymentType paymentType, BigDecimal applicationFee, FeeType applicationFeeType, BigDecimal processingFee,
        FeeType processingFeeType, BigDecimal otherInitialFees, FeeType otherInitialFeesType,
        BigDecimal monthlyManagementFee, FeeType monthlyManagementFeeType, BigDecimal otherMonthlyFees,
        FeeType otherMonthlyFeesType, BigDecimal annualManagementFee, FeeType annualManagementFeeType,
        BigDecimal otherAnnualFees, FeeType otherAnnualFeesType)
    {
        this.LoanAmount = loanAmount;
        this.LoanTermInMonths = loanTermInMonths;
        this.PromotionalPeriodMonths = promotionalPeriodMonths;
        this.AnnualPromotionalInterestRate = annualPromotionalInterestRate;
        this.AnnualInterestRate = annualInterestRate;
        this.GracePeriodMonths = gracePeriodMonths;
        this.PaymentType = paymentType;
        this.ApplicationFee = applicationFee;
        this.ApplicationFeeType = applicationFeeType;
        this.ProcessingFee = processingFee;
        this.ProcessingFeeType = processingFeeType;
        this.OtherInitialFees = otherInitialFees;
        this.OtherInitialFeesType = otherInitialFeesType;
        this.MonthlyManagementFee = monthlyManagementFee;
        this.MonthlyManagementFeeType = monthlyManagementFeeType;
        this.OtherMonthlyFees = otherMonthlyFees;
        this.OtherMonthlyFeesType = otherMonthlyFeesType;
        this.AnnualManagementFee = annualManagementFee;
        this.AnnualManagementFeeType = annualManagementFeeType;
        this.OtherAnnualFees = otherAnnualFees;
        this.OtherAnnualFeesType = otherAnnualFeesType;
    }

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
