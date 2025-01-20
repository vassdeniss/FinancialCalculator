using System.ComponentModel.DataAnnotations;
using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Web.Dto;

using static LeasingConstraints;

public class LeaseInputDto
{
    [Required]
    [Range(MIN_PRICE, MAX_PRICE, ErrorMessage = ERROR_PRICE)]
    public BigDecimal Price { get; init; }

    [Required]
    [Range(MIN_INITIAL_PAYMENT, 999999999, ErrorMessage = ERROR_INITIAL_PAYMENT)]
    public BigDecimal InitialPayment { get; init; }

    [Required]
    [Range(MIN_LEASE_TERM_IN_MONTHS, MAX_LEASE_TERM_IN_MONTHS, ErrorMessage = ERROR_LEASE_TERM_IN_MONTHS)]
    public int LeaseTermInMonths { get; init; }

    [Required]
    [Range(MIN_MONTHLY_INSTALLMENT, MAX_MONTHLY_INSTALLMENT, ErrorMessage = ERROR_MONTHLY_INSTALLMENT)]
    public BigDecimal MonthlyInstallment { get; init; }

    public BigDecimal? InitialProcessingFee { get; init; }
    
    public FeeType? ProcessingFeeType { get; init; }
}