using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.DTO;


namespace FinancialCalculator.Services.UnitTests
{
    [TestFixture]
    public class LeaseValidationTests
    {
        private LeaseService service;

        [SetUp]
        public void Setup()
        {
            service = new LeaseService();
        }

        [TestCase("99", Description = "Price at minimum")]
        [TestCase("1000000000", Description = "Price above maximum")]
        public void Price_OutOfRange_ThrowsArgumentOutOfRangeException(string priceStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = BigDecimal.Parse(priceStr),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Price must be between 100 and 999999999.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("-0.000001", Description = "Initial payment below minimum")]
        public void InitialPayment_BelowMinimum_ThrowsArgumentOutOfRangeException(string initialPaymentStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = new BigDecimal(20000),
                InitialPayment = BigDecimal.Parse(initialPaymentStr),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Initial payment must be at least 0.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("20000", "20000", Description = "Initial payment equals the price")]
        [TestCase("25000", "20000", Description = "Initial payment greater than the price")]
        public void InitialPayment_EqualOrGreaterThanPrice_ThrowsArgumentOutOfRangeException(string initialPaymentStr, string priceStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = BigDecimal.Parse(priceStr),
                InitialPayment = BigDecimal.Parse(initialPaymentStr),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Initial payment cannot equal to the price or be greter.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("100", "100", Description = "Initial processing fee equals the price including VAT")]
        [TestCase("101", "100", Description = "Initial processing fee greater than the price including VAT")]
        public void InitialProcessingFee_EqualOrGreaterThanPriceWithVAT_ThrowsArgumentOutOfRangeException(string initialProcessingFeeStr, string priceStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = BigDecimal.Parse(priceStr),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = BigDecimal.Parse(initialProcessingFeeStr),
                ProcessingFeeType = FeeType.Currency
            };

            string expectedErrorMessage = "Initial processing fee cannot equal to or be greater than the price including VAT.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }


        [TestCase("0", Description = "Lease term below minimum")]
        [TestCase("121", Description = "Lease term above maximum")]
        public void LeaseTerm_OutOfRange_ThrowsArgumentOutOfRangeException(string leaseTermStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = new BigDecimal(20000),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = int.Parse(leaseTermStr),
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Lease month must be between 1 and 120.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("0", Description = "Monthly installment below minimum")]
        [TestCase("999.01", Description = "Monthly installment above maximum")]
        public void MonthlyInstallment_OutOfRange_ThrowsArgumentOutOfRangeException(string monthlyInstallmentStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = new BigDecimal(20000),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr),
                LeaseTermInMonths = 24,
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Monthly installment must be between 0.1 and 999.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("998", "998", Description = "Monthly installment equals the price")]
        [TestCase("999", "998", Description = "Monthly installment greater than the price")]
        public void MonthlyInstallment_EqualOrGreaterThanPrice_ThrowsArgumentOutOfRangeException(string monthlyInstallmentStr, string priceStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = BigDecimal.Parse(priceStr),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr),
                LeaseTermInMonths = 24,
                InitialProcessingFee = new BigDecimal(100)
            };

            string expectedErrorMessage = "Monthly installment must be less than the price.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("-0.01", Description = "Initial processing fee below minimum")]
        [TestCase("50", Description = "Initial processing fee above maximum")]
        public void InitialProcessingFee_Percentage_OutOfRange_ThrowsArgumentOutOfRangeException(string initialProcessingFeeStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = new BigDecimal(20000),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = BigDecimal.Parse(initialProcessingFeeStr),
                ProcessingFeeType = FeeType.Percentage
            };

            string expectedErrorMessage = "Initial processing fee must be between 0 and 49 percent.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("-0.01", Description = "Initial processing fee below minimum")]
        public void InitialProcessingFee_Currency_BelowMinimum_ThrowsArgumentOutOfRangeException(string initialProcessingFeeStr)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = new BigDecimal(20000),
                InitialPayment = new BigDecimal(2000),
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = BigDecimal.Parse(initialProcessingFeeStr),
                ProcessingFeeType = FeeType.Currency
            };

            string expectedErrorMessage = "Initial processing fee must be a valid currency amount and not less than 0.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }


        [TestCase("15000", "25", Description = "Sum of initial payment and processing fee equals the price")]
        [TestCase("15000", "26", Description = "Sum of initial payment and processing fee greater than the price")]
        public void InitialPaymentAndProcessingFee_SumGreaterThanOrEqualToPrice_ThrowsArgumentOutOfRangeException(string initialPaymentStr, string initialProcessingFeePercentageStr)
        {
            // Arrange
            BigDecimal price = new BigDecimal(20000);
            BigDecimal initialPayment = BigDecimal.Parse(initialPaymentStr);
            BigDecimal initialProcessingFeePercentage = BigDecimal.Parse(initialProcessingFeePercentageStr);
            BigDecimal initialProcessingFee = (initialProcessingFeePercentage / new BigDecimal(100)) * price;

            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = price,
                InitialPayment = initialPayment,
                MonthlyInstallment = new BigDecimal(500),
                LeaseTermInMonths = 24,
                InitialProcessingFee = initialProcessingFee
            };

            string expectedErrorMessage = "The sum of initial payment and processing fee must be less than the price.";

            // Act & Assert
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateLeaseResult(input));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }
    }

    [TestFixture]
    public class CalculateLeaseResultTests
    {
        private LeaseService service;

        [SetUp]
        public void Setup()
        {
            service = new LeaseService();
        }

        [TestCase("999999999", "509999999", "999", 120, "0.49", "518786155759795000000000000000000000000000000", "1000179878.51", "489999999.51")]
        [TestCase("100", "0.0000001", "1", 1, "0.001", "-100", "1.1", "0.1")]

        public void CalculateLeaseResult_ValidInput_ReturnsExpectedResults(
      string price, string initialPayment, string monthlyInstallment, int leaseTermInMonths,
      string initialProcessingFeePercentage, string expectedAnnualCostPercent,
      string expectedTotalPaid, string expectedTotalFees)
        {
            // Arrange
            LeaseServiceInputDto input = new LeaseServiceInputDto
            {
                Price = BigDecimal.Parse(price),
                InitialPayment = BigDecimal.Parse(initialPayment),
                MonthlyInstallment = BigDecimal.Parse(monthlyInstallment),
                LeaseTermInMonths = leaseTermInMonths,
                InitialProcessingFee = BigDecimal.Parse(initialProcessingFeePercentage) * BigDecimal.Parse(price)
            };

            // Expected results
            BigDecimal expectedAnnualCostPercentValue = BigDecimal.Parse(expectedAnnualCostPercent);
            BigDecimal expectedTotalPaidValue = BigDecimal.Parse(expectedTotalPaid);
            BigDecimal expectedTotalFeesValue = BigDecimal.Parse(expectedTotalFees);

            // Act
            LeaseResultDto result = service.CalculateLeaseResult(input);

            // Assert
            Assert.That(result.AnnualCostPercent, Is.EqualTo(expectedAnnualCostPercentValue.BankersRounding(2)));
            Assert.That(result.TotalPaid, Is.EqualTo(expectedTotalPaidValue.BankersRounding(2)));
            Assert.That(result.TotalFees, Is.EqualTo(expectedTotalFeesValue.BankersRounding(2)));
        }
    }

    [TestFixture]
    public class APRCalculatorTests
    {
        private LeaseService leaseService;

        [SetUp]
        public void Setup()
        {
            leaseService = new LeaseService();
        }


        [TestCase("240", "1", "0", 6, "-99.99")]
        [TestCase("240", "2", "0", 6, "-99.97")]
        [TestCase("240", "3", "0", 6, "-99.93")]
        [TestCase("240", "4", "0", 6, "-99.85")]
        [TestCase("240", "5", "0", 6, "-99.75")]
        [TestCase("240", "6", "0", 6, "-99.60")]
        public void CalculateNegativeApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
        {
            // Arrange
            BigDecimal financedAmount = BigDecimal.Parse(financedAmountStr);
            BigDecimal monthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr);
            BigDecimal initialFee = BigDecimal.Parse(initialFeeStr);
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);

            // Act
            BigDecimal result = leaseService.CalculateApr(financedAmount, monthlyInstallment, initialFee, leaseTermInMonths);

            // Assert
            Assert.That(result.BankersRounding(2), Is.EqualTo(expectedApr));
        }

        [TestCase("240", "50", "0", 6, "119.57")]
        [TestCase("240", "51", "0", 6, "135.96")]
        [TestCase("240", "52", "0", 6, "153.32")]
        [TestCase("240", "53", "0", 6, "171.68")]
        [TestCase("240", "54", "0", 6, "191.09")]
        [TestCase("240", "55", "0", 6, "211.61")]
        public void CalculatePositiveApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
        {
            // Arrange
            BigDecimal financedAmount = BigDecimal.Parse(financedAmountStr);
            BigDecimal monthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr);
            BigDecimal initialFee = BigDecimal.Parse(initialFeeStr);
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);

            // Act
            BigDecimal result = leaseService.CalculateApr(financedAmount, monthlyInstallment, initialFee, leaseTermInMonths);

            // Assert
            Assert.That(result.BankersRounding(2), Is.EqualTo(expectedApr));
        }

        [TestCase("240", "40", "0", 6, "0")]
        [TestCase("1200", "200", "0", 6, "0")]
        public void CalculateZeroApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
        {
            // Arrange
            BigDecimal financedAmount = BigDecimal.Parse(financedAmountStr);
            BigDecimal monthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr);
            BigDecimal initialFee = BigDecimal.Parse(initialFeeStr);
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);

            // Act
            BigDecimal result = leaseService.CalculateApr(financedAmount, monthlyInstallment, initialFee, leaseTermInMonths);

            // Assert
            Assert.That(result.BankersRounding(2), Is.EqualTo(expectedApr));
        }
    }
}
