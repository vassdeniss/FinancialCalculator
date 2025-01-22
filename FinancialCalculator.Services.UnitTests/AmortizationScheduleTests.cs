using System.Collections.Generic;
using NUnit.Framework;
using FinancialCalculator.Common;
using FinancialCalculator.Services;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.UnitTests
{
    [TestFixture]
    public class AmortizationScheduleTests
    {
        [TestCase("1000", "120", new[] { "336.11", "336.11", "336.12" }, "128.4868")]
        [TestCase("1000", "120", new[] { "337.50", "336.11", "334.73" }, "128.7534")]
        public void CalculateApr_CorrectlyCalculatesApr(string loanAmountStr, string totalInitialFeesStr, string[] payments, string expectedAprStr)
        {
            // Arrange
            BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
            BigDecimal totalInitialFees = BigDecimal.Parse(totalInitialFeesStr);
            var schedule = new List<AmortizationEntry>();
            foreach (var paymentStr in payments)
            {
                schedule.Add(new AmortizationEntry { Payment = BigDecimal.Parse(paymentStr), Fees = BigDecimal.Parse("0") });
            }
            var amortizationSchedule = new AmortizationSchedule();

            // Act
            BigDecimal apr = amortizationSchedule.CalculateApr(loanAmount, totalInitialFees, schedule);

            // Assert
            BigDecimal expectedApr = BigDecimal.Parse(expectedAprStr);
            Assert.That(apr.BankersRounding(4), Is.EqualTo(expectedApr));
        }
    }
}
