using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.DTO;

public class LeaseServiceInputDto
{
    public BigDecimal Price { get; init; }

    public BigDecimal InitialPayment { get; init; }

    public int LeaseTermInMonths { get; init; }

    public BigDecimal MonthlyInstallment { get; init; }

    public BigDecimal InitialProcessingFee { get; init; }
    
    public FeeType? ProcessingFeeType { get; init; }
}
