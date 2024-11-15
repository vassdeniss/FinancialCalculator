using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.Contracts;

public interface ICreditService
{
    int LoanAmount { get; set; }
    int LoanTermInMonths { get; set; }
    decimal InterestRate { get; set; }
    PaymentType PaymentType { get; set; }

    decimal CalculateInitialFees(
        int loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType);
    decimal CalculateAPR();
    decimal CalculateMonthlyPayment();
    decimal CalculateTotalInterestPaid();
    decimal CalculateTotalPayments();
    decimal CalculateTotalPaidWithInterestAndFees();
}