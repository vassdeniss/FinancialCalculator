using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.UnitTests
{
    public class BaseTest
    {
        protected CreditService service;
        protected CreditServiceInputDto serviceInput;

        [SetUp]
        public void Setup()
        {
            service = new CreditService();
            serviceInput = CreateDefaultCreditServiceInputDto();
        }

        protected static CreditServiceInputDto CreateDefaultCreditServiceInputDto()
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
    }

    [TestFixture]
    public class CreditValidationTests : BaseTest
    {
        [TestCase("99", Description = "Loan amount below minimum")]
        [TestCase("1000000000", Description = "Loan amount above maximum")]
        public void LoanAmount_OutOfRange_ThrowsArgumentOutOfRangeException(string loanAmountStr)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmountStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Loan amount must be between 1 and 999999999 BGN. (Parameter '{nameof(serviceInput.LoanAmount)}')"));
        }

        [Test]
        [TestCase(0, Description = "Loan term in months below minimum")]
        [TestCase(961, Description = "Loan term in months above maximum")]
        public void LoanTermInMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int loanTermInMonths)
        {
            // Arrange
            serviceInput.LoanTermInMonths = loanTermInMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Loan month must be between 1 and 960. (Parameter '{nameof(serviceInput.LoanTermInMonths)}')"));
        }

        [Test]
        [TestCase("-1", Description = "Annual interest rate below minimum")]
        [TestCase("10000000", Description = "Annual interest rate above maximum")]
        public void AnnualInterestRate_OutOfRange_ThrowsArgumentOutOfRangeException(string annualInterestRateStr)
        {
            // Arrange
            serviceInput.AnnualInterestRate = BigDecimal.Parse(annualInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Annual interest rate must be between 1 and 9999999%. (Parameter '{nameof(serviceInput.AnnualInterestRate)}')"));
        }

        [Test]
        [TestCase(5, 2, (PaymentType)999, Description = "Invalid payment type with remaining promotional months > 0")]
        [TestCase(1, 2, (PaymentType)999, Description = "Invalid payment type without remaining promotional months <= 0")]
        public void CalculateCreditResult_InvalidPaymentType_ShouldThrowArgumentException(int promotionalPeriodMonths, int gracePeriodMonths, PaymentType paymentType)
        {
            // Arrange
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;
            serviceInput.GracePeriodMonths = gracePeriodMonths;
            serviceInput.PaymentType = paymentType;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Invalid payment type."));
        }

        [Test]
        [TestCase(PaymentType.Annuity, Description = "Valid payment type: Annuity")]
        [TestCase(PaymentType.Decreasing, Description = "Valid payment type: Decreasing")]
        public void CalculateCreditResult_ValidPaymentType_ShouldNotThrowException(PaymentType paymentType)
        {
            // Arrange
            serviceInput.PaymentType = paymentType;

            // Act & Assert
            Assert.DoesNotThrow(() => service.CalculateCreditResult(serviceInput));
        }
    }

    [TestFixture]
    public class PromotionalPeriodValidationTests : BaseTest
    {
        [Test]
        [TestCase(-1, Description = "Promotional period months below minimum")]
        [TestCase(301, Description = "Promotional period months above maximum")]
        public void PromotionalPeriodMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int promotionalPeriodMonths)
        {
            // Arrange
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Promotional loan month must be between 0 and 300. (Parameter '{nameof(serviceInput.PromotionalPeriodMonths)}')"));
        }

        [Test]
        [TestCase(12, 12, Description = "Promotional period months equal to loan term months")]
        [TestCase(12, 13, Description = "Promotional period months greater than loan term months")]
        public void PromotionalPeriodMonths_GreaterThanOrEqualLoanTermInMonths_ThrowsArgumentOutOfRangeException(int loanTermInMonths, int promotionalPeriodMonths)
        {
            // Arrange
            serviceInput.LoanTermInMonths = loanTermInMonths;
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Promotional loan month must be lower than the loan term in months. (Parameter '{nameof(serviceInput.PromotionalPeriodMonths)}')"));
        }

        [Test]
        [TestCase("-1", Description = "Annual promotional interest rate below minimum")]
        [TestCase("41", Description = "Annual promotional interest rate above maximum")]
        public void AnnualPromotionalInterestRate_OutOfRange_ThrowsArgumentOutOfRangeException(string annualPromotionalInterestRateStr)
        {
            // Arrange
            serviceInput.AnnualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Annual promo interest rate must be between 1 and 40%. (Parameter '{nameof(serviceInput.AnnualPromotionalInterestRate)}')"));
        }

        [Test]
        [TestCase(1, "0", Description = "Promotional period months > 0 but annual promotional interest rate = 0")]
        [TestCase(0, "1", Description = "Promotional period months = 0 but annual promotional interest rate > 0")]
        public void PromotionalPeriodMonths_AndAnnualPromotionalInterestRate_InvalidCombination_ThrowsArgumentException(int promotionalPeriodMonths, string annualPromotionalInterestRateStr)
        {
            // Arrange
            serviceInput.PromotionalPeriodMonths = promotionalPeriodMonths;
            serviceInput.AnnualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Both promotional fields need to be higher than 0 if used."));
        }

        [Test]
        [TestCase(-1, Description = "Grace period months below minimum")]
        [TestCase(301, Description = "Grace period months above maximum")]
        public void GracePeriodMonths_OutOfRange_ThrowsArgumentOutOfRangeException(int gracePeriodMonths)
        {
            // Arrange
            serviceInput.GracePeriodMonths = gracePeriodMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Grace period months have to be between 0 and 300. (Parameter '{nameof(serviceInput.GracePeriodMonths)}')"));
        }

        [Test]
        [TestCase(12, 12, Description = "Grace period months equal to loan term months")]
        [TestCase(13, 12, Description = "Grace period months greater than loan term months")]
        public void GracePeriodMonths_EqualOrGreaterThanLoanTerm_ThrowsArgumentOutOfRangeException(int gracePeriodMonths, int loanTermInMonths)
        {
            // Arrange
            serviceInput.GracePeriodMonths = gracePeriodMonths;
            serviceInput.LoanTermInMonths = loanTermInMonths;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo($"Grace period cannot exceed loan term. (Parameter '{nameof(serviceInput.GracePeriodMonths)}')"));
        }
    }

    [TestFixture]
    public class FeesValidationTests : BaseTest
    {
        [Test]
        [TestCase(-1, Description = "Application fee below minimum")]
        public void ApplicationFee_BelowMinimum_ThrowsArgumentOutOfRangeException(int applicationFee)
        {
            // Arrange
            serviceInput.ApplicationFee = BigDecimal.Parse(applicationFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Application fee must be positive if used. (Parameter 'ApplicationFee')"));
        }

        [Test]
        [TestCase(-1, Description = "Processing fee below minimum")]
        public void ProcessingFee_BelowMinimum_ThrowsArgumentOutOfRangeException(int processingFee)
        {
            // Arrange
            serviceInput.ProcessingFee = BigDecimal.Parse(processingFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Processing fee must be positive if used. (Parameter 'ProcessingFee')"));
        }

        [Test]
        [TestCase(-1, Description = "Other initial fees below minimum")]
        public void OtherInitialFees_BelowMinimum_ThrowsArgumentOutOfRangeException(int otherInitialFees)
        {
            // Arrange
            serviceInput.OtherInitialFees = BigDecimal.Parse(otherInitialFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Other initial fees must be positive if used. (Parameter 'OtherInitialFees')"));
        }

        [Test]
        [TestCase(-1, Description = "Monthly management fee below minimum")]
        public void MonthlyManagementFee_BelowMinimum_ThrowsArgumentOutOfRangeException(int monthlyManagementFee)
        {
            // Arrange
            serviceInput.MonthlyManagementFee = BigDecimal.Parse(monthlyManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Monthly management fee must be positive if used. (Parameter 'MonthlyManagementFee')"));
        }

        [Test]
        [TestCase(-1, Description = "Other monthly fees below minimum")]
        public void OtherMonthlyFees_BelowMinimum_ThrowsArgumentOutOfRangeException(int otherMonthlyFees)
        {
            // Arrange
            serviceInput.OtherMonthlyFees = BigDecimal.Parse(otherMonthlyFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Other monthly fees must be positive if used. (Parameter 'OtherMonthlyFees')"));
        }

        [Test]
        [TestCase(-1, Description = "Annual management fee below minimum")]
        public void AnnualManagementFee_BelowMinimum_ThrowsArgumentOutOfRangeException(int annualManagementFee)
        {
            // Arrange
            serviceInput.AnnualManagementFee = BigDecimal.Parse(annualManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Annual management fee must be positive if used. (Parameter 'AnnualManagementFee')"));
        }

        [Test]
        [TestCase(-1, Description = "Other annual fees below minimum")]
        public void OtherAnnualFees_BelowMinimum_ThrowsArgumentOutOfRangeException(int otherAnnualFees)
        {
            // Arrange
            serviceInput.OtherAnnualFees = BigDecimal.Parse(otherAnnualFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Other annual fees must be positive if used. (Parameter 'OtherAnnualFees')"));
        }

        [Test]
        [TestCase(5000, 3000, 2000, 500, Description = "Total initial fees exceed 50% of the loan amount")]
        public void TotalInitialFees_ExceedLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int applicationFee, int processingFee, int otherInitialFees)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.ApplicationFee = BigDecimal.Parse(applicationFee.ToString());
            serviceInput.ProcessingFee = BigDecimal.Parse(processingFee.ToString());
            serviceInput.OtherInitialFees = BigDecimal.Parse(otherInitialFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateCreditResult(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Initial fees have be less than 50% of the loan. (Parameter 'totalInitialFees')"));
        }

        [Test]
        [TestCase(41, Description = "Application fee percentage exceeds 40%")]
        public void ApplicationFee_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int applicationFee)
        {
            // Arrange
            serviceInput.ApplicationFeeType = FeeType.Percentage;
            serviceInput.ApplicationFee = BigDecimal.Parse(applicationFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'ApplicationFee')"));
        }

        [Test]
        [TestCase(41, Description = "Processing fee percentage exceeds 40%")]
        public void ProcessingFee_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int processingFee)
        {
            // Arrange
            serviceInput.ProcessingFeeType = FeeType.Percentage;
            serviceInput.ProcessingFee = BigDecimal.Parse(processingFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'ProcessingFee')"));
        }

        [Test]
        [TestCase(41, Description = "Other initial fees percentage exceeds 40%")]
        public void OtherInitialFees_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int otherInitialFees)
        {
            // Arrange
            serviceInput.OtherInitialFeesType = FeeType.Percentage;
            serviceInput.OtherInitialFees = BigDecimal.Parse(otherInitialFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'OtherInitialFees')"));
        }

        [Test]
        [TestCase(5000, 5000, Description = "Monthly management fee (currency) exceeds loan amount")]
        public void MonthlyManagementFee_Currency_ExceedsLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int monthlyManagementFee)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.MonthlyManagementFeeType = FeeType.Currency;
            serviceInput.MonthlyManagementFee = BigDecimal.Parse(monthlyManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Currency fields (monthly and annual fees) must be less than the loan amount. (Parameter 'MonthlyManagementFee')"));
        }

        [Test]
        [TestCase(5000, 5000, Description = "Other monthly fees (currency) exceed loan amount")]
        public void OtherMonthlyFees_Currency_ExceedsLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int otherMonthlyFees)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.OtherMonthlyFeesType = FeeType.Currency;
            serviceInput.OtherMonthlyFees = BigDecimal.Parse(otherMonthlyFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Currency fields (monthly and annual fees) must be less than the loan amount. (Parameter 'OtherMonthlyFees')"));
        }

        [Test]
        [TestCase(41, Description = "Monthly management fee percentage exceeds 40%")]
        public void MonthlyManagementFee_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int monthlyManagementFee)
        {
            // Arrange
            serviceInput.MonthlyManagementFeeType = FeeType.Percentage;
            serviceInput.MonthlyManagementFee = BigDecimal.Parse(monthlyManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'MonthlyManagementFee')"));
        }

        [Test]
        [TestCase(41, Description = "Other monthly fees percentage exceeds 40%")]
        public void OtherMonthlyFees_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int otherMonthlyFees)
        {
            // Arrange
            serviceInput.OtherMonthlyFeesType = FeeType.Percentage;
            serviceInput.OtherMonthlyFees = BigDecimal.Parse(otherMonthlyFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'OtherMonthlyFees')"));
        }

        [Test]
        [TestCase(5000, 5000, Description = "Annual management fee (currency) exceeds loan amount")]
        public void AnnualManagementFee_Currency_ExceedsLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int annualManagementFee)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.AnnualManagementFeeType = FeeType.Currency;
            serviceInput.AnnualManagementFee = BigDecimal.Parse(annualManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Currency fields (monthly and annual fees) must be less than the loan amount. (Parameter 'AnnualManagementFee')"));
        }

        [Test]
        [TestCase(5000, 5000, Description = "Other annual fees (currency) exceed loan amount")]
        public void OtherAnnualFees_Currency_ExceedsLoanAmount_ThrowsArgumentOutOfRangeException(int loanAmount, int otherAnnualFees)
        {
            // Arrange
            serviceInput.LoanAmount = BigDecimal.Parse(loanAmount.ToString());
            serviceInput.OtherAnnualFeesType = FeeType.Currency;
            serviceInput.OtherAnnualFees = BigDecimal.Parse(otherAnnualFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("Currency fields (monthly and annual fees) must be less than the loan amount. (Parameter 'OtherAnnualFees')"));
        }

        [Test]
        [TestCase(41, Description = "Annual management fee percentage exceeds 40%")]
        public void AnnualManagementFee_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int annualManagementFee)
        {
            // Arrange
            serviceInput.AnnualManagementFeeType = FeeType.Percentage;
            serviceInput.AnnualManagementFee = BigDecimal.Parse(annualManagementFee.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'AnnualManagementFee')"));
        }

        [Test]
        [TestCase(41, Description = "Other annual fees percentage exceeds 40%")]
        public void OtherAnnualFees_Percentage_ExceedsLimit_ThrowsArgumentOutOfRangeException(int otherAnnualFees)
        {
            // Arrange
            serviceInput.OtherAnnualFeesType = FeeType.Percentage;
            serviceInput.OtherAnnualFees = BigDecimal.Parse(otherAnnualFees.ToString());

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.ValidateFees(serviceInput));
            Assert.That(ex.Message, Is.EqualTo("A single fee field cannot exceed 40% of the loan amount. (Parameter 'OtherAnnualFees')"));
        }
    }
}

