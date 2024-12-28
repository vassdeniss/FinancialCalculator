using FinancialCalculator.Common;

namespace FinancialCalculator.Services.UnitTests;

[Category("PaymentCalculationService")]
[TestFixture]
public class CalculateMonthlyPaymentTests
{
    private PaymentCalculationService service;

    [SetUp]
    public void Setup()
    {
        service = new PaymentCalculationService();
    }

    [Test]
    [TestCase("105", "25.0", 2, "54.15")] // High interest rate, low loan amount, low payments
    [TestCase("200", "30.0", 800, "5.00")] // High interest rate, low loan amount, high payments
    [TestCase("99999988", "20.0", 4, "26050271.89")] // High interest rate, high loan amount, low payments
    [TestCase("888888888", "27.0", 750, "20000001.11")] // High interest rate, high loan amount, high payments
    [TestCase("788899666", "2.0", 700, "1910277.25")] // Low interest rate, high loan amount, high payments -- fail
    [TestCase("500", "3.0", 780, "1.46")] // Low interest rate, low loan amount, high payments
    [TestCase("700000500", "1.0", 10, "70321284.44")] // Low interest rate, high loan amount, low payments
    [TestCase("200", "2.0", 12, "16.85")] // Low interest rate, low loan amount, low payments
    [TestCase("100", "5.0", 1, "100.42")] // Minimum loan amount, minimum payments
    [TestCase("9999999", "4.0", 360, "47741.52")] // Upper boundary of valid loan amount
    [TestCase("10000", "9999999", 12, "83333325.00")] // Max interest rate
    public void CalculateMonthlyPayment_ShouldReturnCorrectValue(string loanAmountStr, string annualInterestRateStr, int payments, string expectedMonthlyPaymentStr)
    {
        // Arrange
        BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
        BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);

        // Act
        BigDecimal result = service.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments).Round(2);

        // Assert
        Assert.That(result, Is.EqualTo(BigDecimal.Parse(expectedMonthlyPaymentStr)));
    }

    [Test]
    [TestCase("999999999", 960, "1041666.67")]  // Max loan, max months, zero interest rate
    [TestCase("100", 960, "0.10")] // Min loan, max months, zero interest rate
    [TestCase("100", 1, "100")] // Min loan, min months, zero interest rate
    [TestCase("999999999", 1, "999999999.00")] // Max loan, min months, zero interest rate
    public void CalculateMonthlyPayment_ShouldReturnCorrectValue_WhenInterestRateIsZero(string loanAmountStr, int payments, string expectedMonthlyPaymentStr)
    {
        // Arrange
        BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);

        // Act
        BigDecimal result = service.CalculateMonthlyPayment(loanAmount, BigDecimal.Zero, payments).Round(2);

        // Assert
        Assert.That(result, Is.EqualTo(BigDecimal.Parse(expectedMonthlyPaymentStr)));
    }

    [Test]
    [TestCase("999999999", "1259.82", 960, "1049849998.95")] // Edge case: max loan, max months, max interest rate
    [TestCase("100", "1301.47", 960, "108.46")] // Edge case: min loan, max months, max interest rate
    [TestCase("100", "9999999", 1, "833433.25")]  // Edge case: min loan, min months, max interest rate
    [TestCase("999999999", "9999999", 1, "8334332491665.67")] // Edge case: max loan, min months, max interest rate
    public void CalculateMonthlyPayment_ShouldReturnCorrectValue_ForMaxPossibleInterestValues(string loanAmountStr, string annualInterestRateStr, int payments, string expectedMonthlyPaymentStr)
    {
        // Assert
        BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
        BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);

        // Act
        BigDecimal result = service.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments).Round(2);

        // Assert
        Assert.That(result, Is.EqualTo(BigDecimal.Parse(expectedMonthlyPaymentStr)));
    }
}

[Category("PaymentCalculationService")]
[TestFixture]
public class CalculateRemainingBalanceTests
{

    private PaymentCalculationService service;

    [SetUp]
    public void Setup()
    {
        service = new PaymentCalculationService();
    }

    // Note: Some of these tests fail due to precision issues.
    // The precision limitation accumulates over time, resulting in greater discrepancies as the loan term in months increases.
    // This is particularly evident in scenarios with a large number of payments.
    //Also the method must be optimize because it can't load with big payments
    // Also the current implementation struggles with high monthly payments,
    // causing the website to hang or not respond when clicking "Calculate."
    // We should consider optimizing the method to handle large inputs efficiently.
    [Test]
    [TestCase("777777777", "30", 2, "403532235.54", "-0.01")]
    [TestCase("777777777", "30", 3, "272328907.58", "0.01")]
    [TestCase("777777777", "30", 4, "206747238.02", "-0.02")]
    [TestCase("777777777", "30", 5, "167414224.98", "0.01")]
    [TestCase("777777777", "30", 6, "141205532.91", "-0.03")]
    [TestCase("777777777", "30", 7, "122496445.16", "-0.04")]
    [TestCase("777777777", "30", 8, "108474602.21", "-0.05")]
    [TestCase("777777777", "30", 9, "97577581.02", "0.02")]
    [TestCase("777777777", "30", 10, "88867926.83", "-0.05")]
    [TestCase("777777777", "30", 11, "81749076.61", "0.03")]
    [TestCase("777777777", "30", 12, "75823320.92", "-0.08")]
    [TestCase("777777777", "30", 13, "70815321.68", "-0.02")]
    [TestCase("777777777", "30", 14, "66528408.20", "-0.09")]
    [TestCase("777777777", "30", 15, "62818354.64", "0.07")]
    [TestCase("777777777", "30", 16, "59576991.08", "-0.03")] //Within(0.01)
    [TestCase("777777777", "30", 17, "56721598.76", "-0.03")]
    [TestCase("777777777", "30", 18, "54187840.37", "0.06")]
    [TestCase("777777777", "30", 19, "51924922.83", "-0.08")]
    [TestCase("777777777", "30", 20, "49892211.19", "-0.06")]
    [TestCase("777777777", "30", 21, "48056810.08", "0.11")]
    [TestCase("777777777", "30", 22, "46391804.71", "0.11")]
    [TestCase("777777777", "30", 23, "44874960.73", "-0.13")]
    [TestCase("777777777", "30", 24, "43487749.13", "-0.16")]
    [TestCase("777777777", "30", 25, "42214605.22", "-0.17")] // Within(0.01)
    [TestCase("777777777", "30", 100, "21242572.27", "-2.03999999911")] // Within(0.01)
    [TestCase("777777777", "30", 137, "20127767.44", "-2.38000000268")] //Within(0.26)
    [TestCase("777777777", "30", 288, "19460318.07", "23.870000001")] //Within(-1.91)
    [TestCase("777777777", "30", 480, "19444582.89", "4646.44")] //Within(-1035.47)
    [TestCase("777777777", "30", 900, "19444444.43", "-226973862.01")] //Within(24791610.92)
    [TestCase("777777777", "30", 960, "19444444.43", "-3642918185")] //Within(109077875.66)
    public void CalculateRemainingBalance_ShouldReturnCorrectValue_WhenIntrestRateIsAtMax(string loanAmountStr, string annualInterestRateStr, int payments, string monthlyPaymentStr, string expectedRemainingBalanceStr)
    {
        // Arrange
        BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
        BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);
        BigDecimal monthlyPayment = BigDecimal.Parse(monthlyPaymentStr);
        BigDecimal expectedRemainingBalance = BigDecimal.Parse(expectedRemainingBalanceStr);

        // Act
        BigDecimal result = service.CalculateRemainingBalance(loanAmount, annualInterestRate, payments, monthlyPayment);

        // Assert
        Assert.That(result, Is.EqualTo(expectedRemainingBalance.Round(2)));
    }
}

[TestFixture]
[Category("PaymentCalculationService")]
public class CalculateMonthlyPaymentWithPromotionalTests
{
    private PaymentCalculationService service;

    [SetUp]
    public void Setup()
    {
        service = new PaymentCalculationService();
    }

    [Test]
    [TestCase("999999999", 300, "40", "0", 960, "33333333.30", "1515151.51")]
    [TestCase("999999999", 300, "40", "2208.76", 960, "33333333.30", "1840633331.49")]
    [TestCase("100", 300, "40", "2293.03", 960, "3.33", "191.09")]
    [TestCase("100", 300, "40", "0", 960, "3.33", "0.15")]
    public void CalculateMonthlyPaymentWithPromotional_ShouldReturnCorrectValues_WhenPromotionIsAtMax(string loanAmountStr, int promotionalPeriodMonths, string annualPromotionalInterestRateStr, string annualInterestRateStr, int loanTermInMonths, string expectedPromoMonthlyPaymentStr, string expectedNormalMonthlyPaymentStr)
    {
        // Arrange
        BigDecimal loanAmount = BigDecimal.Parse(loanAmountStr);
        BigDecimal annualPromotionalInterestRate = BigDecimal.Parse(annualPromotionalInterestRateStr);
        BigDecimal annualInterestRate = BigDecimal.Parse(annualInterestRateStr);
        BigDecimal expectedPromoMonthlyPayment = BigDecimal.Parse(expectedPromoMonthlyPaymentStr);
        BigDecimal expectedNormalMonthlyPayment = BigDecimal.Parse(expectedNormalMonthlyPaymentStr);

        // Act
        var result = service.CalculateMonthlyPaymentWithPromotional(
            loanAmount,
            promotionalPeriodMonths,
            annualPromotionalInterestRate,
            annualInterestRate,
            loanTermInMonths);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Item1, Is.EqualTo(expectedPromoMonthlyPayment));
            Assert.That(result.Item2, Is.EqualTo(expectedNormalMonthlyPayment));
        });

    }
}

[TestFixture]
[Category("PaymentCalculationService")]
public class CalculateMonthlyPaymentWithoutPromotional
{
    private PaymentCalculationService service;

    [SetUp]
    public void Setup()
    {
        service = new PaymentCalculationService();
    }
}
