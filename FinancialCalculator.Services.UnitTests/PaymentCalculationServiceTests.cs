using FinancialCalculator.Services;

namespace FinancialCalculator.Tests
{
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
        [TestCase(105, 25.0, 2, 54.14)] // High interest rate, low loan amount, low payments
        [TestCase(200, 30.0, 800, 5.25)] // High interest rate, low loan amount, high payments
        [TestCase(99999988, 20.0, 4, 26050271.89)] // High interest rate, high loan amount, low payments
        [TestCase(888888888, 27.0, 750, 20001633.74)] // High interest rate, high loan amount, high payments
        [TestCase(788899666, 2.0, 700, 1910277.26)] // Low interest rate, high loan amount, high payments -- fail
        [TestCase(500, 3.0, 780, 1.45)] // Low interest rate, low loan amount, high payments
        [TestCase(700000500, 1.0, 10, 70321284.44)] // Low interest rate, high loan amount, low payments
        [TestCase(200, 2.0, 12, 16.85)] // Low interest rate, low loan amount, low payments
        [TestCase(10000, 5.1234567, 12, 856.64)] // High precision interest rate
        [TestCase(100, 5.0, 1, 100.42)] // Minimum loan amount, minimum payments
        [TestCase(9999999, 4.0, 360, 47741.53)] // Upper boundary of valid loan amount
        [TestCase(10000, 9999999, 12, 83334158.33)] // Max interest rate
        public void CalculateMonthlyPayment_ShouldReturnCorrectValue(decimal loanAmount, double annualInterestRate, int payments, decimal expectedMonthlyPayment)
        {
            // Act
            decimal result = service.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMonthlyPayment).Within(0.01));
        }

        [Test]
        [TestCase(999999999, 0, 960, 1041666.67)]  // Max loan, max months, zero interest rate
        [TestCase(100, 0, 960, 0.10)] // Min loan, max months, zero interest rate
        [TestCase(100, 0, 1, 100)] // Min loan, min months, zero interest rate
        [TestCase(999999999, 0, 1, 999999999)] // Max loan, min months, zero interest rate
        public void CalculateMonthlyPayment_ShouldReturnCorrectValue_WhenInterestRateIsZero(decimal loanAmount, double annualInterestRate, int payments, decimal expectedMonthlyPayment)
        {
            // Act
            decimal result = service.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMonthlyPayment).Within(0.01));
        }

        [Test]
        [TestCase(999999999, 1259.82, 960, 1050891665.62)] // Edge case: max loan, max months, max interest rate
        [TestCase(100, 1301.47, 960, 108.56)] // Edge case: min loan, max months, max interest rate
        [TestCase(100, 9999999, 1, 833433.25)]  // Edge case: min loan, min months, max interest rate
        [TestCase(999999999, 9999999, 1, 8334332491665.67)] // Edge case: max loan, min months, max interest rate
        public void CalculateMonthlyPayment_ShouldReturnCorrectValue_ForMaxPossibleInterestValues(decimal loanAmount, double annualInterestRate, int payments, decimal expectedMonthlyPayment)
        {
            // Act
            decimal result = service.CalculateMonthlyPayment(loanAmount, annualInterestRate, payments);

            // Assert
            Assert.That(result, Is.EqualTo(expectedMonthlyPayment).Within(0.01));
        }
    }
}
