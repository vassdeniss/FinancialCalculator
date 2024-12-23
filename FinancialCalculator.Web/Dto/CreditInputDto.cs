using System.ComponentModel.DataAnnotations;
using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Web.Attribute;
using static FinancialCalculator.Common.CreditConstraints;

namespace FinancialCalculator.Web.Dto;

public class CreditInputDto
{
    [Required]
    [BigDecimalRange(MIN_LOAN_AMOUNT, int.MaxValue, ErrorMessage = ERROR_LOAN_AMOUNT)]
    public BigDecimal LoanAmount { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_LOAN_TERM_IN_MONTHS, MAX_LOAN_TERM_IN_MONTHS, ErrorMessage = ERROR_LOAN_TERM_IN_MONTHS)]
    public int LoanTermInMonths { get; init; }
    
    [BigDecimalRange(MIN_PROMO_LOAN_TERM_IN_MONTHS, MAX_PROMO_LOAN_TERM_IN_MONTHS, ErrorMessage = ERROR_PROMO_LOAN_TERM_IN_MONTHS)]
    public int? PromotionalPeriodMonths { get; init; }
    
    [BigDecimalRange(MIN_ANNUAL_PROMO_INTEREST_RATE_AMOUNT, int.MaxValue, ErrorMessage = ERROR_ANNUAL_PROMO_INTEREST_RATE_AMOUNT)]
    public BigDecimal? AnnualPromotionalInterestRate { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_ANNUAL_INTEREST_RATE_AMOUNT, int.MaxValue, ErrorMessage = ERROR_ANNUAL_INTEREST_RATE_AMOUNT)]
    public BigDecimal AnnualInterestRate { get; init; }

    [BigDecimalRange(MIN_GRACE_PERIOD_MONTHS, MAX_GRACE_PERIOD_MONTHS, ErrorMessage = ERROR_GRACE_PERIOD_MONTHS)]
    public int? GracePeriodMonths { get; init; }
    
    [Required(ErrorMessage = "Payment type must be provided.")]
    public PaymentType PaymentType { get; init; }
    
    public BigDecimal? ApplicationFee { get; init; }
    public FeeType? ApplicationFeeType { get; init; }
    
    public BigDecimal? ProcessingFee { get; init; }
    public FeeType? ProcessingFeeType { get; init; }
    
    public BigDecimal? OtherInitialFees { get; init; }
    public FeeType? OtherInitialFeesType { get; init; }
    
    public BigDecimal? MonthlyManagementFee { get; init; }
    public FeeType? MonthlyManagementFeeType { get; init; }
    
    public BigDecimal? OtherMonthlyFees { get; init; }
    public FeeType? OtherMonthlyFeesType { get; init; }
    
    public BigDecimal? AnnualManagementFee { get; init; }
    public FeeType? AnnualManagementFeeType { get; init; }
    
    public BigDecimal? OtherAnnualFees { get; init; }
    public FeeType? OtherAnnualFeesType { get; init; }
}
