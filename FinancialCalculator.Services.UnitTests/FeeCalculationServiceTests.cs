using System;
using NUnit.Framework;
using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services;

namespace FinancialCalculator.Services.UnitTests
{
    [TestFixture]
    public class FeeCalculationServiceTests
    {
        private FeeCalculationService feeCalculationService;

        [SetUp]
        public void Setup()
        {
            feeCalculationService = new FeeCalculationService();
        }

        [Test]
        [TestCase("0.0000000001", FeeType.Percentage, "100", "0.0000000001", Description = "Calculates the lowest possible percentage fee for the smallest possible loan amount")]
        [TestCase("0.0000000001", FeeType.Currency, "100", "0.0000000001", Description = "Handles the lowest possible currency fee for the smallest possible loan amount")]
        [TestCase("0.0000000001", FeeType.Percentage, "999999999", "0.000999999999", Description = "Calculates the lowest possible percentage fee for the maximum loan amount")]
        [TestCase("0.0000000001", FeeType.Currency, "999999999", "0.0000000001", Description = "Handles the lowest possible currency fee for the maximum loan amount")]

        [TestCase("40", FeeType.Percentage, "100", "40", Description = "Calculates the maximum possible percentage fee for the smallest possible loan amount")]
        [TestCase("499999999.49", FeeType.Currency, "100", "499999999.49", Description = "Handles the maximum possible currency fee for the smallest possible loan amount")]
        [TestCase("40", FeeType.Percentage, "999999999", "399999999.6", Description = "Calculates the maximum possible percentage fee for the maximum loan amount")]
        [TestCase("499999999.49", FeeType.Currency, "999999999", "499999999.49", Description = "Handles the maximum possible currency fee for the maximum loan amount")]

        public void CalculateFee_ValidInputs_ReturnsCorrectFee(string feeValueStr, FeeType feeType, string loanAmountStr, string expectedFeeStr)
        {
            // Arrange
            BigDecimal feeValue = BigDecimal.Parse(feeValueStr);
            BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);

            // Act
            BigDecimal result = feeCalculationService.CalculateFee(feeValue, feeType, loanAmount);

            // Assert
            BigDecimal expectedFee = BigDecimal.Parse(expectedFeeStr);
            Assert.That(result, Is.EqualTo(expectedFee));
        }

        [Test]
        [TestCase("1000", "1", "10", Description = "Calculates early repayment fee for 1% of 1000")]
        [TestCase("1000", "5", "50", Description = "Calculates early repayment fee for 5% of 1000")]
        [TestCase("1000", "10", "100", Description = "Calculates early repayment fee for 10% of 1000")]
        [TestCase("1000", "0", "0", Description = "Calculates early repayment fee for 0% of 1000")]
        [TestCase("0", "10", "0", Description = "Calculates early repayment fee for 10% of 0")]
        public void CalculateEarlyRepaymentFee_ValidInputs_ReturnsCorrectFee(string outstandingPrincipalStr, string earlyRepaymentFeePercentStr, string expectedFeeStr)
        {
            // Arrange
            BigDecimal outstandingPrincipal = BigDecimal.Parse(outstandingPrincipalStr);
            BigDecimal earlyRepaymentFeePercent = BigDecimal.Parse(earlyRepaymentFeePercentStr);
            BigDecimal expectedFee = BigDecimal.Parse(expectedFeeStr);

            // Act
            BigDecimal result = feeCalculationService.CalculateEarlyRepaymentFee(outstandingPrincipal, earlyRepaymentFeePercent);

            // Assert
            Assert.That(result, Is.EqualTo(expectedFee));
        }

        [Test]
        [TestCase("1000", "0", "0", "0", Description = "Initial fee is 0% and 0 currency")]
        [TestCase("1000", "1", "0", "10", Description = "Initial fee is 1% and 0 currency")]
        [TestCase("1000", "5", "0", "50", Description = "Initial fee is 5% and 0 currency")]
        [TestCase("1000", "0", "10", "10", Description = "Initial fee is 0% and 10 currency")]
        [TestCase("1000", "1", "10", "20", Description = "Initial fee is 1% and 10 currency")]
        [TestCase("1000", "5", "10", "60", Description = "Initial fee is 5% and 10 currency")]

        public void CalculateInitialFees_ValidInputs_ReturnsCorrectFee(string newLoanPrincipalStr, string initialFeePercentStr, string initialFeeCurrencyStr, string expectedFeeStr)
        {
            // Arrange
            BigDecimal newLoanPrincipal = BigDecimal.Parse(newLoanPrincipalStr);
            BigDecimal initialFeePercent = BigDecimal.Parse(initialFeePercentStr);
            BigDecimal initialFeeCurrency = BigDecimal.Parse(initialFeeCurrencyStr);
            BigDecimal expectedFee = BigDecimal.Parse(expectedFeeStr);

            // Act
            BigDecimal result = feeCalculationService.CalculateInitialFees(newLoanPrincipal, initialFeePercent, initialFeeCurrency);

            // Assert
            Assert.That(result, Is.EqualTo(expectedFee));
        }
    }
}
