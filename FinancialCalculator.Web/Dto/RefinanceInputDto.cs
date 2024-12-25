using System.ComponentModel.DataAnnotations;
using FinancialCalculator.Common;
using FinancialCalculator.Web.Attribute;
using static FinancialCalculator.Common.RefinanceConstraints;

namespace FinancialCalculator.Web.Dto;

public class RefinanceInputDto
{
    [Required]
    [BigDecimalRange(MIN_LOAN_AMOUNT, MAX_LOAN_AMOUNT, ErrorMessage = ERROR_LOAN_AMOUNT)]
    public BigDecimal LoanAmount { get; init; }
    
    [Required]
    [Range(MIN_LOAN_TERM_IN_MONTHS, MAX_LOAN_TERM_IN_MONTHS, ErrorMessage = ERROR_LOAN_TERM_IN_MONTHS)]
    public int LoanTermInMonths { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_ANNUAL_INTEREST_RATE_AMOUNT, MAX_ANNUAL_INTEREST_RATE_AMOUNT, ErrorMessage = ERROR_ANNUAL_INTEREST_RATE_AMOUNT)]
    public BigDecimal AnnualInterestRate { get; init; }
    
    [Required]
    [Range(MIN_CONTRIBUTIONS_MADE, int.MaxValue, ErrorMessage = ERROR_CONTRIBUTIONS_MADE)]
    public int ContributionsMade { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_EARLY_REPAYMENT_FEE, MAX_EARLY_REPAYMENT_FEE, ErrorMessage = ERROR_EARLY_REPAYMENT_FEE)]
    public BigDecimal EarlyRepaymentFee { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_ANNUAL_INTEREST_RATE_AMOUNT, MAX_ANNUAL_INTEREST_RATE_AMOUNT, ErrorMessage = ERROR_ANNUAL_INTEREST_RATE_AMOUNT)]
    public BigDecimal NewAnnualInterestRate { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_INITIAL_FEE, MAX_INITIAL_FEE, ErrorMessage = ERROR_INITIAL_FEE)]
    public BigDecimal InitialFee { get; init; }
    
    [Required]
    [BigDecimalRange(MIN_INITIAL_FEE, MAX_INITIAL_FEE, ErrorMessage = ERROR_INITIAL_FEE)]
    public BigDecimal InitialFeeCurrency { get; init; }
}