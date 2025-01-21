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

        //TODO Write Edge Cases when validation is set

        [Test]
        [TestCase("5", FeeType.Percentage, "1000", "50")]
        [TestCase("499999999.6", FeeType.Currency, "999999999", "499999999.6")]
        [TestCase("50.01", FeeType.Currency, "100", "50.01")]
        public void CalculateFee_PercentageFeeType_ReturnsCorrectFee(string feeValueStr, FeeType feeType, string loanAmountStr, string expectedFeeStr)
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
        public void CalculateInitialFees_ValidInputs_ReturnsCorrectInitialFees()
        {
            // Arrange
            BigDecimal newLoanPrincipal = new BigDecimal(10000); 
            BigDecimal initialFeePercent = new BigDecimal(2);
            BigDecimal initialFeeCurrency = new BigDecimal(150);

            // Act
            BigDecimal result = feeCalculationService.CalculateInitialFees(newLoanPrincipal, initialFeePercent, initialFeeCurrency);

            // Assert
            BigDecimal expectedInitialFees = new BigDecimal(350);
            Assert.That(result, Is.EqualTo(expectedInitialFees));
        }

        [Test]
        public void CalculateEarlyRepaymentFee_ValidInputs_ReturnsCorrectRepaymentFee()
        {
            // Arrange
            BigDecimal outstandingPrincipal = new BigDecimal(5000);
            BigDecimal earlyRepaymentFeePercent = new BigDecimal(3);

            // Act
            BigDecimal result = feeCalculationService.CalculateEarlyRepaymentFee(outstandingPrincipal, earlyRepaymentFeePercent);

            // Assert
            BigDecimal expectedRepaymentFee = new BigDecimal(150);
            Assert.That(result, Is.EqualTo(expectedRepaymentFee));
        }
    }
}
