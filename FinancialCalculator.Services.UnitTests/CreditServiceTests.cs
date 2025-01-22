/*using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.UnitTests
{
    public class CreditServiceTests
    {
        CreditService service;
        CreditServiceInputDto defaultServiceInput;

        [SetUp]
        public void Setup()
        {
            service = new CreditService();
            defaultServiceInput = CreateDefaultCreditServiceInputDto();
        }

        public static CreditServiceInputDto CreateDefaultCreditServiceInputDto()
        {
            return new CreditServiceInputDto(
                loanAmount: BigDecimal.Parse("5000"),
                loanTermInMonths: 24,
                promotionalPeriodMonths: 6,
                annualPromotionalInterestRate: BigDecimal.Parse("2"),
                annualInterestRate: BigDecimal.Parse("5"),
                gracePeriodMonths: 3,
                paymentType: PaymentType.Annuity,
                applicationFee: BigDecimal.Parse("100"),
                applicationFeeType: FeeType.Currency,
                processingFee: BigDecimal.Parse("100"),
                processingFeeType: FeeType.Currency,
                otherInitialFees: BigDecimal.Parse("50"),
                otherInitialFeesType: FeeType.Currency,
                monthlyManagementFee: BigDecimal.Parse("10"),
                monthlyManagementFeeType: FeeType.Currency,
                otherMonthlyFees: BigDecimal.Parse("10"),
                otherMonthlyFeesType: FeeType.Currency,
                annualManagementFee: BigDecimal.Parse("100"),
                annualManagementFeeType: FeeType.Currency,
                otherAnnualFees: BigDecimal.Parse("50"),
                otherAnnualFeesType: FeeType.Currency
            );
        }

        [Test]
        [TestCase("99")]
        [TestCase("1000000000")]
        public void LoanAmount_OutOfRange_ThrowsArgumentOutOfRangeException(string loanAmountStr)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmountStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Loan amount must be between 1 and 999999999 BGN. (Parameter '{nameof(serviceInput.LoanAmount)}')"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(961)]
        public void LoanTermInMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int loanTermInMonths)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanTermInMonths = loanTermInMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Loan month must be between 1 and 960. (Parameter '{nameof(serviceInput.LoanTermInMonths)}')"));
        }

        [Test]
        [TestCase("-1")]
        [TestCase("10000000")]
        public void AnnualInterestRate_OutOfRange_ThrowsArgumentOutOfRangeException(string annualInterestRateStr)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.AnnualInterestRate = BigDecimal.Parse(annualInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Annual interest rate must be between 1 and 9999999%. (Parameter '{nameof(serviceInput.AnnualInterestRate)}')"));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void PromotionalPeriodMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int promotionalPeriodMonths)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Promotional loan month must be between 0 and 300. (Parameter '{nameof(serviceInput.PromotionalPeriodMonths)}')"));
        }

        [Test]
        [TestCase(12, 12)]
        [TestCase(12, 13)]
        public void PromotionalPeriodMonths_GreaterThanOrEqualLoanTermInMonths_ThrowsArgumentOutOfRangeException(int loanTermInMonths, int promotionalPeriodMonths)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanTermInMonths = loanTermInMonths;
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Promotional loan month must be lower than the loan term in months. (Parameter '{nameof(serviceInput.PromotionalPeriodMonths)}')"));
        }

        [Test]
        [TestCase("-1")]
        [TestCase("41")]
        public void AnnualPromotionalInterestRate_OutOfRange_ThrowsArgumentOutOfRangeException(string annualPromotionalInterestRateStr)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.AnnualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Annual promo interest rate must be between 1 and 40%. (Parameter '{nameof(serviceInput.AnnualPromotionalInterestRate)}')"));
        }

        [Test]
        [TestCase(1, "0")]
        [TestCase(0, "1")]
        public void PromotionalPeriodMonths_AndAnnualPromotionalInterestRate_InvalidCombination_ThrowsArgumentException(int promotionalPeriodMonths, string annualPromotionalInterestRateStr)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;
            serviceInput.AnnualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Both promotional fields need to be higher than 0 if used."));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void GracePeriodMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int gracePeriodMonths)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.GracePeriodMonths = gracePeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Grace period months have to be between 0 and 300. (Parameter '{nameof(serviceInput.GracePeriodMonths)}')"));
        }

        [Test]
        [TestCase(12, 12)]
        [TestCase(13, 12)]
        public void GracePeriodMonths_EqualOrGreaterThanLoanTerm_ThrowsArgumentOutOfRangeException(int gracePeriodMonths, int loanTermInMonths)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.GracePeriodMonths = gracePeriodMonths;
            serviceInput.LoanTermInMonths = loanTermInMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Grace period months must be less than the loan term in months. (Parameter '{nameof(serviceInput.GracePeriodMonths)}')"));
        }

        [Test]
        [TestCase(100, 50, FeeType.Percentage, 50, FeeType.Percentage, 50, FeeType.Percentage, 50, FeeType.Percentage, 50, FeeType.Percentage, 50, FeeType.Percentage, 50, FeeType.Percentage)]
        public void AllFeePercentageFields_ExceedMaxPercentage_ThrowsArgumentOutOfRangeException(
     int loanAmount,
     int applicationFee, FeeType applicationFeeType,
     int processingFee, FeeType processingFeeType,
     int otherInitialFees, FeeType otherInitialFeesType,
     int monthlyManagementFee, FeeType monthlyManagementFeeType,
     int otherMonthlyFees, FeeType otherMonthlyFeesType,
     int annualManagementFee, FeeType annualManagementFeeType,
     int otherAnnualFees, FeeType otherAnnualFeesType)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.ApplicationFee = BigDecimal.Parse(applicationFee.ToString());
            serviceInput.ApplicationFeeType = applicationFeeType;
            serviceInput.ProcessingFee = BigDecimal.Parse(processingFee.ToString());
            serviceInput.ProcessingFeeType = processingFeeType;
            serviceInput.OtherInitialFees = BigDecimal.Parse(otherInitialFees.ToString());
            serviceInput.OtherInitialFeesType = otherInitialFeesType;
            serviceInput.MonthlyManagementFee = BigDecimal.Parse(monthlyManagementFee.ToString());
            serviceInput.MonthlyManagementFeeType = monthlyManagementFeeType;
            serviceInput.OtherMonthlyFees = BigDecimal.Parse(otherMonthlyFees.ToString());
            serviceInput.OtherMonthlyFeesType = otherMonthlyFeesType;
            serviceInput.AnnualManagementFee = BigDecimal.Parse(annualManagementFee.ToString());
            serviceInput.AnnualManagementFeeType = annualManagementFeeType;
            serviceInput.OtherAnnualFees = BigDecimal.Parse(otherAnnualFees.ToString());
            serviceInput.OtherAnnualFeesType = otherAnnualFeesType;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"A single fee field shouldn't exceed 40% of the loan amount. (Parameter '{nameof(serviceInput.ApplicationFee)}' or '{nameof(serviceInput.ProcessingFee)}' or '{nameof(serviceInput.OtherInitialFees)}' or '{nameof(serviceInput.MonthlyManagementFee)}' or '{nameof(serviceInput.OtherMonthlyFees)}' or '{nameof(serviceInput.AnnualManagementFee)}' or '{nameof(serviceInput.OtherAnnualFees)}')"));
        }

        [Test]
        [TestCase(100, 200, FeeType.Currency, 15, FeeType.Currency, 30, FeeType.Currency, 2, FeeType.Currency)]
        [TestCase(200, 5, FeeType.Currency, 300, FeeType.Currency, 5, FeeType.Currency, 9999999, FeeType.Currency)]
        [TestCase(50, 5, FeeType.Currency, 2, FeeType.Currency, 10, FeeType.Currency, 9999999, FeeType.Currency)]
        [TestCase(50, 5, FeeType.Currency, 2, FeeType.Currency, 10500, FeeType.Currency, 1, FeeType.Currency)]
        public void MonthlyAndAnnualCurrencyFees_LessThanLoanAmount_ThrowsArgumentOutOfRangeException(
    int loanAmount,
    int monthlyManagementFee, FeeType monthlyManagementFeeType,
    int otherMonthlyFees, FeeType otherMonthlyFeesType,
    int annualManagementFee, FeeType annualManagementFeeType,
    int otherAnnualFees, FeeType otherAnnualFeesType)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.MonthlyManagementFee = BigDecimal.Parse(monthlyManagementFee.ToString());
            serviceInput.MonthlyManagementFeeType = monthlyManagementFeeType;
            serviceInput.OtherMonthlyFees = BigDecimal.Parse(otherMonthlyFees.ToString());
            serviceInput.OtherMonthlyFeesType = otherMonthlyFeesType;
            serviceInput.AnnualManagementFee = BigDecimal.Parse(annualManagementFee.ToString());
            serviceInput.AnnualManagementFeeType = annualManagementFeeType;
            serviceInput.OtherAnnualFees = BigDecimal.Parse(otherAnnualFees.ToString());
            serviceInput.OtherAnnualFeesType = otherAnnualFeesType;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Currency fields (monthly and annual fees) must be less than the loan amount. (Parameter '{nameof(serviceInput.MonthlyManagementFee)}' or '{nameof(serviceInput.OtherMonthlyFees)}' or '{nameof(serviceInput.AnnualManagementFee)}' or '{nameof(serviceInput.OtherAnnualFees)}')"));
        }


        [Test]
        [TestCase(100, 30, FeeType.Currency, 45, FeeType.Currency, 30, FeeType.Currency)]
        public void CombinedInitialFees_ExceedLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int applicationFee, FeeType applicationFeeType, int processingFee, FeeType processingFeeType, int otherInitialFees, FeeType otherInitialFeesType)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.ApplicationFee = BigDecimal.Parse(applicationFee.ToString());
            serviceInput.ApplicationFeeType = applicationFeeType;
            serviceInput.ProcessingFee = BigDecimal.Parse(processingFee.ToString());
            serviceInput.ProcessingFeeType = processingFeeType;
            serviceInput.OtherInitialFees = BigDecimal.Parse(otherInitialFees.ToString());
            serviceInput.OtherInitialFeesType = otherInitialFeesType;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"The combined initial fees must not exceed half of the loan amount. (Parameter '{nameof(serviceInput.ApplicationFee)}' or '{nameof(serviceInput.ProcessingFee)}' or '{nameof(serviceInput.OtherInitialFees)}')"));
        }

        [Test]
        [TestCase(5, (PaymentType)999)]
        [TestCase(1, (PaymentType)999)]
        public void CalculateCreditResult_InvalidPaymentType_ShouldThrowArgumentException(int promotionalPeriodMonths, PaymentType paymentType)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;
            serviceInput.PaymentType = paymentType;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Invalid payment type."));
        }

        [Test]
        [TestCase(PaymentType.Annuity)]
        [TestCase(PaymentType.Decreasing)]
        public void CalculateCreditResult_ValidPaymentType_ShouldNotThrowException(PaymentType paymentType)
        {
            // Arrange
            CreditServiceInputDto serviceInput = defaultServiceInput;
            serviceInput.PaymentType = paymentType;

            // Act & Assert
            Assert.DoesNotThrow(() => service.CalculateCreditResult(serviceInput));
        }
    }
}*/

