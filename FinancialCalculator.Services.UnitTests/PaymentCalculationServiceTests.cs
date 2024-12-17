using FinancialCalculator.Common;

namespace FinancialCalculator.Services.UnitTests;

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

[TestFixture]
public class CalculateRemainingBalanceTests
{

    private PaymentCalculationService service;

    [SetUp]
    public void Setup()
    {
        service = new PaymentCalculationService();
    }
}