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

        [TestCase("240", "1", "0", 6, "-99.75")]
        public void CalculateApr_ValidInput_ReturnsExpectedResult(string financedAmountStr, string monthlyInstallmentStr, string initialFeeStr, int leaseTermInMonths, string expectedAprStr)
        {
            // Arrange
            BigDecimal financedAmount = BigDecimal.Parse(financedAmountStr);
            BigDecimal monthlyInstallment = BigDecimal.Parse(monthlyInstallmentStr);
            BigDecimal initialFee = BigDecimal.Parse(initialFeeStr);
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);

            // Act
            BigDecimal result = leaseService.CalculateApr(financedAmount, monthlyInstallment, initialFee, leaseTermInMonths);

            // Assert
            Assert.That(result, Is.EqualTo(expectedApr));
        }
    }
}
