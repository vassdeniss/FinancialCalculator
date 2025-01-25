using FinancialCalculator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialCalculator.Services.UnitTests
{
    public class LeaseServiceTests
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
        public void CalculateApr_NegativeApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
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

        [TestCase("240", "49", "0", 6, "104.1")]
        [TestCase("240", "50", "0", 6, "119.57")]
        [TestCase("240", "51", "0", 6, "135.96")]
        [TestCase("240", "52", "0", 6, "153.32")]
        [TestCase("240", "53", "0", 6, "171.68")]
        [TestCase("240", "54", "0", 6, "191.09")]
        [TestCase("240", "55", "0", 6, "211.61")]
        public void CalculateApr_PositiveApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
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
        public void CalculateApr_ZeroApr_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
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
