using FinancialCalculator.Common;
using FinancialCalculator.Services.DTO;
using FinancialCalculator.Common.Enums;

namespace FinancialCalculator.Services.UnitTests
{
    [TestFixture]
    public class AmortizationScheduleTests
    {
        [TestCase("1000", "120", "5", 12, "33.94")]
        [TestCase("1000", "120", "5", 36, "14.65")]
        //[TestCase("999999999", "0","1259.82", 960, "550282.9694")]
        public void CalculateApr_ForAnnuityPayments(string loanAmountStr, string totalInitialFeesStr, string annualInterestRateStr, int termInMonths, string expectedAprStr)
        {
            // Arrange
            BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
            BigDecimal totalInitialFees = BigDecimal.Parse(totalInitialFeesStr);
            BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);
            var schedule = new List<AmortizationEntry>();

            var paymentCalculationService = new PaymentCalculationService();
            BigDecimal monthlyPayment = paymentCalculationService.CalculateMonthlyPayment(loanAmount, annualInterestRate, termInMonths);

            for (int i = 0; i < termInMonths; i++)
            {
                schedule.Add(new AmortizationEntry { Payment = monthlyPayment, Fees = BigDecimal.Zero });
            }

            var amortizationSchedule = new AmortizationSchedule();

            // Act
            BigDecimal apr = amortizationSchedule.CalculateApr(loanAmount, totalInitialFees, schedule);

            // Assert
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);
            Assert.That(apr.BankersRounding(2), Is.EqualTo(expectedApr));
        }

        /*[TestCase("1000", 12, 3, 3, "5", "10", "100", "105",
                  PaymentType.Annuity, "10", FeeType.Currency, "20", FeeType.Currency, "30", FeeType.Currency, "10", FeeType.Currency,
                  "5", FeeType.Currency, "0", FeeType.Currency, "0", FeeType.Currency, "120",
                  "1250", "300", "700", "50", "128.4868")]
        public void GenerateAmortizationSchedule_Test(
    string loanAmountStr, int loanTermInMonths, int gracePeriodMonths, int remainingPromotionalMonths,
    string annualPromotionalInterestRateStr, string annualInterestRateStr, string monthlyPaymentPromoStr, string monthlyPaymentNormalStr,
    PaymentType paymentType, string applicationFeeStr, FeeType applicationFeeType, string processingFeeStr, FeeType processingFeeType,
    string otherInitialFeesStr, FeeType otherInitialFeesType, string monthlyManagementFeeStr, FeeType monthlyManagementFeeType,
    string otherMonthlyFeesStr, FeeType otherMonthlyFeesType, string annualManagementFeeStr, FeeType annualManagementFeeType,
    string otherAnnualFeesStr, FeeType otherAnnualFeesType, string totalInitialFeesStr, string expectedTotalPaymentsStr,
    string expectedTotalInterestStr, string expectedTotalPrincipalStr, string expectedTotalFeesStr, string expectedAprStr)
        {
            // Arrange
            BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
            BigDecimal annualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);
            BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);
            BigDecimal monthlyPaymentPromo = BigDecimal.Parse(monthlyPaymentPromoStr);
            BigDecimal monthlyPaymentNormal = BigDecimal.Parse(monthlyPaymentNormalStr);
            BigDecimal totalInitialFees = BigDecimal.Parse(totalInitialFeesStr);

            var serviceInput = new CreditServiceInputDto(
                loanAmount, loanTermInMonths, remainingPromotionalMonths, annualPromotionalInterestRate,
                annualInterestRate, gracePeriodMonths, paymentType, BigDecimal.Parse(applicationFeeStr), applicationFeeType,
                BigDecimal.Parse(processingFeeStr), processingFeeType, BigDecimal.Parse(otherInitialFeesStr), otherInitialFeesType,
                BigDecimal.Parse(monthlyManagementFeeStr), monthlyManagementFeeType, BigDecimal.Parse(otherMonthlyFeesStr),
                otherMonthlyFeesType, BigDecimal.Parse(annualManagementFeeStr), annualManagementFeeType, BigDecimal.Parse(otherAnnualFeesStr),
                otherAnnualFeesType
            );

            var amortizationSchedule = new AmortizationSchedule();

            // Act
            CreditResultDto result = amortizationSchedule.GenerateAmortizationSchedule(
                loanAmount, loanTermInMonths, gracePeriodMonths, remainingPromotionalMonths,
                annualPromotionalInterestRate, annualInterestRate, monthlyPaymentPromo, monthlyPaymentNormal,
                serviceInput, totalInitialFees);

            // Assert
            BigDecimal expectedTotalPayments = BigDecimal.Parse(expectedTotalPaymentsStr);
            BigDecimal expectedTotalInterest = BigDecimal.Parse(expectedTotalInterestStr);
            BigDecimal expectedTotalPrincipal = BigDecimal.Parse(expectedTotalPrincipalStr);
            BigDecimal expectedTotalFees = BigDecimal.Parse(expectedTotalFeesStr);
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);

            Assert.That(result.TotalPayments, Is.EqualTo(expectedTotalPayments));
            Assert.That(result.TotalInterest, Is.EqualTo(expectedTotalInterest));
            Assert.That(result.TotalPrincipal, Is.EqualTo(expectedTotalPrincipal));
            Assert.That(result.TotalFees, Is.EqualTo(expectedTotalFees));
            Assert.That(result.Apr.BankersRounding(4), Is.EqualTo(expectedApr));
        }*/

    }
}
