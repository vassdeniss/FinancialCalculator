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

    public BigDecimal LoanAmount { get; init; }
    
    public int LoanTermInMonths { get; init; }
    
    public int PromotionalPeriodMonths { get; init; }
    
    public BigDecimal AnnualPromotionalInterestRate { get; init; }
    
    public BigDecimal AnnualInterestRate { get; init; }

    public int GracePeriodMonths { get; init; }
    
    public PaymentType PaymentType { get; init; }
    
    public BigDecimal ApplicationFee { get; init; }
    public FeeType ApplicationFeeType { get; init; }
    
    public BigDecimal ProcessingFee { get; init; }
    public FeeType ProcessingFeeType { get; init; }
    
    public BigDecimal OtherInitialFees { get; init; }
    public FeeType OtherInitialFeesType { get; init; }
    
    public BigDecimal MonthlyManagementFee { get; init; }
    public FeeType MonthlyManagementFeeType { get; init; }
    
    public BigDecimal OtherMonthlyFees { get; init; }
    public FeeType OtherMonthlyFeesType { get; init; }
    
    public BigDecimal AnnualManagementFee { get; init; }
    public FeeType AnnualManagementFeeType { get; init; }
    
    public BigDecimal OtherAnnualFees { get; init; }
    public FeeType OtherAnnualFeesType { get; init; }
}
