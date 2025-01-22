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
    }
}
