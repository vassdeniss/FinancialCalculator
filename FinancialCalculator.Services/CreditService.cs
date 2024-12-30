using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;

using static FinancialCalculator.Common.CreditConstraints;

namespace FinancialCalculator.Services;

public class CreditService : ICreditService
{
    private readonly IFeeCalculationService _feeCalculationService = new FeeCalculationService();
    private readonly IPaymentCalculationService _paymentCalculationService = new PaymentCalculationService();
    private readonly IAmortizationSchedule _amortizationSchedule = new AmortizationSchedule();

    /// <inheritdoc />
    public CreditResultDto CalculateCreditResult(CreditServiceInputDto serviceInput)
    {
        if (serviceInput.LoanAmount.CompareTo(new BigDecimal(MIN_LOAN_AMOUNT)) < 0 ||
            serviceInput.LoanAmount.CompareTo(new BigDecimal(MAX_LOAN_AMOUNT)) > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.LoanAmount), ERROR_LOAN_AMOUNT);
        }

        if (serviceInput.LoanTermInMonths < MIN_LOAN_TERM_IN_MONTHS 
            || serviceInput.LoanTermInMonths > MAX_LOAN_TERM_IN_MONTHS)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.LoanTermInMonths), ERROR_LOAN_TERM_IN_MONTHS);
        }

        if (serviceInput.AnnualInterestRate.CompareTo(new BigDecimal(MIN_ANNUAL_INTEREST_RATE_AMOUNT)) < 0
            || serviceInput.AnnualInterestRate.CompareTo(new BigDecimal(MAX_ANNUAL_INTEREST_RATE_AMOUNT)) > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.AnnualInterestRate), ERROR_ANNUAL_INTEREST_RATE_AMOUNT);
        }

        if (serviceInput.PromotionalPeriodMonths < MIN_PROMO_LOAN_TERM_IN_MONTHS ||
            serviceInput.PromotionalPeriodMonths > MAX_PROMO_LOAN_TERM_IN_MONTHS)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.PromotionalPeriodMonths), ERROR_PROMO_LOAN_TERM_IN_MONTHS);
        }

        if (serviceInput.PromotionalPeriodMonths >= serviceInput.LoanTermInMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.PromotionalPeriodMonths),
                ERROR_PROMO_LOAN_TERM_IN_MONTHS_TOO_HIGH);
        }

        if (serviceInput.AnnualPromotionalInterestRate.CompareTo(new BigDecimal(MIN_ANNUAL_PROMO_INTEREST_RATE_AMOUNT)) < 0
            || serviceInput.AnnualPromotionalInterestRate.CompareTo(new BigDecimal(MAX_ANNUAL_PROMO_INTEREST_RATE_AMOUNT)) > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.AnnualPromotionalInterestRate),
                ERROR_ANNUAL_PROMO_INTEREST_RATE_AMOUNT);
        }

        if ((serviceInput.PromotionalPeriodMonths > MIN_ANNUAL_INTEREST_RATE_AMOUNT && serviceInput.AnnualPromotionalInterestRate.CompareTo(BigDecimal.Zero) == 0) ||
            (serviceInput.PromotionalPeriodMonths == 0 &&
             serviceInput.AnnualPromotionalInterestRate.CompareTo(new BigDecimal(MIN_ANNUAL_PROMO_INTEREST_RATE_AMOUNT)) > 0))
        {
            throw new ArgumentException(ERROR_PROMOTIONAL_FIELDS);
        }

        if (serviceInput.GracePeriodMonths < MIN_GRACE_PERIOD_MONTHS ||
            serviceInput.GracePeriodMonths > MAX_GRACE_PERIOD_MONTHS)
        {
            throw new ArgumentOutOfRangeException(nameof(serviceInput.GracePeriodMonths), ERROR_GRACE_PERIOD_MONTHS);
        }

        BigDecimal totalInitialFees =
            this._feeCalculationService.CalculateFee(serviceInput.ApplicationFee, serviceInput.ApplicationFeeType, serviceInput.LoanAmount)
            + this._feeCalculationService.CalculateFee(serviceInput.ProcessingFee, serviceInput.ProcessingFeeType, serviceInput.LoanAmount)
            + this._feeCalculationService.CalculateFee(serviceInput.OtherInitialFees, serviceInput.OtherInitialFeesType, serviceInput.LoanAmount);

        // Adjust repayment term after grace period
        int repaymentTerm = serviceInput.LoanTermInMonths - serviceInput.GracePeriodMonths;

        // Determine remaining promotional months after grace period
        int remainingPromotionalMonths = serviceInput.PromotionalPeriodMonths > serviceInput.GracePeriodMonths
            ? serviceInput.PromotionalPeriodMonths - serviceInput.GracePeriodMonths
            : 0;

        Tuple<BigDecimal, BigDecimal> resultPayments;
        if (repaymentTerm > 0)
        {
            if (remainingPromotionalMonths > 0)
            {
                // Calculate payments with remaining promotional period
                resultPayments = serviceInput.PaymentType switch
                {
                    PaymentType.Annuity => this._paymentCalculationService.CalculateMonthlyPaymentWithPromotional(
                        serviceInput.LoanAmount,
                        remainingPromotionalMonths,
                        serviceInput.AnnualPromotionalInterestRate,
                        serviceInput.AnnualInterestRate,
                        repaymentTerm),

                    PaymentType.Decreasing => new Tuple<BigDecimal, BigDecimal>(serviceInput.LoanAmount / new BigDecimal(repaymentTerm), BigDecimal.Zero),

                    _ => throw new ArgumentException("Invalid payment type.")
                };
            }
            else
            {
                // No promotional period after grace period
                resultPayments = serviceInput.PaymentType switch
                {
                    PaymentType.Annuity => this._paymentCalculationService.CalculateMonthlyPaymentWithoutPromotional(
                        serviceInput.LoanAmount,
                        serviceInput.AnnualInterestRate,
                        repaymentTerm),

                    PaymentType.Decreasing => new Tuple<BigDecimal, BigDecimal>(serviceInput.LoanAmount / new BigDecimal(repaymentTerm), BigDecimal.Zero),

                    _ => throw new ArgumentException("Invalid payment type.")
                };
            }
        }
        else
        {
            resultPayments = new Tuple<BigDecimal, BigDecimal>(BigDecimal.Zero, BigDecimal.Zero);
        }

        return this._amortizationSchedule.GenerateAmortizationSchedule(
            serviceInput.LoanAmount,
            serviceInput.LoanTermInMonths,
            serviceInput.GracePeriodMonths,
            remainingPromotionalMonths,
            serviceInput.AnnualPromotionalInterestRate,
            serviceInput.AnnualInterestRate,
            resultPayments.Item1,
            resultPayments.Item2,
            serviceInput,
            totalInitialFees);
    }
}
