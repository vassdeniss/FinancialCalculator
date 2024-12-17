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
    public CreditResultDto CalculateCreditResult(CreditInputDto input)
    {
        if (input.LoanAmount.CompareTo(new BigDecimal(MinLoanAmount)) < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.LoanAmount), ErrorLoanAmount);
        }

        if (input.LoanTermInMonths < MinLoanTermInMonths 
            || input.LoanTermInMonths > MaxLoanTermInMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(input.LoanTermInMonths), ErrorLoanTermInMonths);
        }

        if (input.AnnualInterestRate.CompareTo(new BigDecimal(MinAnnualInterestRateAmount)) < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.AnnualInterestRate), ErrorAnnualInterestRateAmount);
        }

        if (input.PromotionalPeriodMonths < MinPromoLoanTermInMonths ||
            input.PromotionalPeriodMonths > MaxPromoLoanTermInMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(input.PromotionalPeriodMonths), ErrorPromoLoanTermInMonths);
        }

        if (input.PromotionalPeriodMonths >= input.LoanTermInMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(input.PromotionalPeriodMonths),
                ErrorPromoLoanTermInMonthsTooHigh);
        }

        if (input.AnnualPromotionalInterestRate.CompareTo(new BigDecimal(MinAnnualPromoInterestRateAmount)) < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(input.AnnualPromotionalInterestRate),
                ErrorAnnualPromoInterestRateAmount);
        }

        if ((input.PromotionalPeriodMonths > MinAnnualInterestRateAmount && input.AnnualPromotionalInterestRate.CompareTo(BigDecimal.Zero) == 0) ||
            (input.PromotionalPeriodMonths == 0 &&
             input.AnnualPromotionalInterestRate.CompareTo(new BigDecimal(MinAnnualPromoInterestRateAmount)) > 0))
        {
            throw new ArgumentException(ErrorPromotionalFields);
        }

        if (input.GracePeriodMonths < MinGracePeriodMonths ||
            input.GracePeriodMonths > MaxGracePeriodMonths)
        {
            throw new ArgumentOutOfRangeException(nameof(input.GracePeriodMonths), ErrorGracePeriodMonths);
        }

        BigDecimal totalInitialFees =
            this._feeCalculationService.CalculateFee(input.ApplicationFee, input.ApplicationFeeType, input.LoanAmount)
            + this._feeCalculationService.CalculateFee(input.ProcessingFee, input.ProcessingFeeType, input.LoanAmount)
            + this._feeCalculationService.CalculateFee(input.OtherInitialFees, input.OtherInitialFeesType, input.LoanAmount);

        // Adjust repayment term after grace period
        int repaymentTerm = input.LoanTermInMonths - input.GracePeriodMonths;

        // Determine remaining promotional months after grace period
        int remainingPromotionalMonths = input.PromotionalPeriodMonths > input.GracePeriodMonths
            ? input.PromotionalPeriodMonths - input.GracePeriodMonths
            : 0;

        Tuple<BigDecimal, BigDecimal> resultPayments;
        if (repaymentTerm > 0)
        {
            if (remainingPromotionalMonths > 0)
            {
                // Calculate payments with remaining promotional period
                resultPayments = input.PaymentType switch
                {
                    PaymentType.Annuity => this._paymentCalculationService.CalculateMonthlyPaymentWithPromotional(
                        input.LoanAmount,
                        remainingPromotionalMonths,
                        input.AnnualPromotionalInterestRate,
                        input.AnnualInterestRate,
                        repaymentTerm),

                    PaymentType.Decreasing => new Tuple<BigDecimal, BigDecimal>(input.LoanAmount / new BigDecimal(repaymentTerm), BigDecimal.Zero),

                    _ => throw new ArgumentException("Invalid payment type.")
                };
            }
            else
            {
                // No promotional period after grace period
                resultPayments = input.PaymentType switch
                {
                    PaymentType.Annuity => this._paymentCalculationService.CalculateMonthlyPaymentWithoutPromotional(
                        input.LoanAmount,
                        input.AnnualInterestRate,
                        repaymentTerm),

                    PaymentType.Decreasing => new Tuple<BigDecimal, BigDecimal>(input.LoanAmount / new BigDecimal(repaymentTerm), BigDecimal.Zero),

                    _ => throw new ArgumentException("Invalid payment type.")
                };
            }
        }
        else
        {
            resultPayments = new Tuple<BigDecimal, BigDecimal>(BigDecimal.Zero, BigDecimal.Zero);
        }

        return this._amortizationSchedule.GenerateAmortizationSchedule(
            input.LoanAmount,
            input.LoanTermInMonths,
            input.GracePeriodMonths,
            remainingPromotionalMonths,
            input.AnnualPromotionalInterestRate,
            input.AnnualInterestRate,
            resultPayments.Item1,
            resultPayments.Item2,
            input,
            totalInitialFees);
    }
}
