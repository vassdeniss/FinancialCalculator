using FinancialCalculator.Services.Enums;

namespace FinancialCalculator.Services.UnitTests;

[TestFixture]
public class CreditServiceTests
{
    private CreditService _creditService;

    [SetUp]
    public void Setup()
    {
        this._creditService = new CreditService();
    }

    [Test]
    public void LoanAmount_SetToGreaterThanMax_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.LoanAmount = 1000000000; // Greater than maximum allowed
        });
        Assert.That(ex.Message, Does.Contain("Loan amount must be between 100 and 999,999,999."));
    }

    [Test]
    public void LoanAmount_SetToLessThanMin_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.LoanAmount = 99; // Less than minimum allowed
        });
        Assert.That(ex.Message, Does.Contain("Loan amount must be between 100 and 999,999,999."));
    }

    [Test]
    public void LoanAmount_SetToValidValue_ShouldNotThrowException()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => this._creditService.LoanAmount = 5000);
        Assert.That(this._creditService.LoanAmount, Is.EqualTo(5000));
    }

    [Test]
    public void LoanTermInMonths_SetToLessThanMin_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.LoanTermInMonths = 0; // Less than minimum allowed
        });
        Assert.That(ex.Message, Does.Contain("Loan term must be between 1 and 960 months."));
    }

    [Test]
    public void LoanTermInMonths_SetToGreaterThanMax_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.LoanTermInMonths = 961; // Greater than maximum allowed
        });
        Assert.That(ex.Message, Does.Contain("Loan term must be between 1 and 960 months."));
    }

    [Test]
    public void LoanTermInMonths_SetToValidValue_ShouldNotThrowException()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => this._creditService.LoanTermInMonths = 24);
        Assert.That(this._creditService.LoanTermInMonths, Is.EqualTo(24));
    }

    [Test]
    public void InterestRate_SetToLessThanMin_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.InterestRate = -0.01m; // Less than minimum allowed
        });
        Assert.That(ex.Message, Does.Contain("Interest rate must be between 0 and 9,999,999."));
    }

    [Test]
    public void InterestRate_SetToGreaterThanMax_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            this._creditService.InterestRate = 10000000m; // Greater than maximum allowed
        });
        Assert.That(ex.Message, Does.Contain("Interest rate must be between 0 and 9,999,999."));
    }

    [Test]
    public void InterestRate_SetToValidValue_ShouldNotThrowException()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => this._creditService.InterestRate = 5.5m);
        Assert.That(this._creditService.InterestRate, Is.EqualTo(5.5m));
    }

    [Test]
    public void CalculateMonthlyPayment_Annuity_ShouldReturnExpectedValue()
    {
        // Arrange
        this._creditService.LoanAmount = 1000;
        this._creditService.LoanTermInMonths = 12;
        this._creditService.InterestRate = 0.12m;
        this._creditService.PaymentType = PaymentType.Annuity;
        decimal expectedMonthlyPayment = 88.85m;

        // Act
        decimal result = this._creditService.CalculateAverageMonthlyPayment();

        // Assert
        Assert.That(result, Is.EqualTo(expectedMonthlyPayment));
    }

    [Test]
    public void CalculateAverageMonthlyPayment_ShouldReturnExpectedValue()
    {
        // Arrange
        this._creditService.LoanAmount = 1000;
        this._creditService.LoanTermInMonths = 12;
        this._creditService.InterestRate = 0.12m;
        this._creditService.PaymentType = PaymentType.Decreasing;
        decimal expectedAverageMonthlyPayment = 88.75m;

        // Act
        decimal result = this._creditService.CalculateAverageMonthlyPayment();

        // Assert
        Assert.That(result, Is.EqualTo(expectedAverageMonthlyPayment));
    }

    [TestCase(10000, 100, FeeType.Currency, 5, FeeType.Percentage, 50, FeeType.Currency, 650)]
    [TestCase(5000, 50, FeeType.Currency, 2, FeeType.Percentage, 25, FeeType.Currency, 175)]
    [TestCase(30000, 0.01, FeeType.Percentage, 5, FeeType.Percentage, 0.01, FeeType.Percentage, 1506)]
    [TestCase(35000, 200, FeeType.Currency, 0.01, FeeType.Currency, 150, FeeType.Currency, 350.01)]
    public void CalculateInitialFees_WithValidData_ShouldReturnCorrectTotalFees(
        int loanAmount,
        decimal? applicationFee, FeeType applicationFeeType,
        decimal? processingFee, FeeType processingFeeType,
        decimal? otherFees, FeeType otherFeesType,
        decimal? expectedTotalInitialFees)
    {
        // Act
        decimal result = this._creditService.CalculateInitialFees(loanAmount, applicationFee, applicationFeeType, processingFee, processingFeeType, otherFees, otherFeesType);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTotalInitialFees));
    }


    [Test]
    public void CalculateInitialFees_WithLoanAmountLessThanMin_ShouldThrowException()
    {
        // Arrange
        int loanAmount = 99; // Less than minimum allowed
        decimal? applicationFee = 100m;
        decimal? processingFee = 5m;
        decimal? otherFees = 50m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            this._creditService.CalculateInitialFees(loanAmount, applicationFee, FeeType.Currency, processingFee, FeeType.Percentage, otherFees, FeeType.Currency);
        }, "Loan amount must be greater than or equal to 100.");
    }

    [TestCase(0, FeeType.Currency)]
    [TestCase(0, FeeType.Percentage)]
    public void CalculateInitialFees_WithZeroForApplicationFee_ShouldThrowException(decimal applicationFee, FeeType applicationFeeType)
    {
        // Arrange
        int loanAmount = 10000;
        decimal? processingFee = 5m;
        FeeType processingFeeType = FeeType.Percentage;
        decimal? otherFees = 50m;
        FeeType otherFeesType = FeeType.Currency;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            this._creditService.CalculateInitialFees(loanAmount, applicationFee, applicationFeeType, processingFee, processingFeeType, otherFees, otherFeesType);
        }, "Application fee must be greater than zero.");
    }

    [TestCase(0, FeeType.Currency)]
    [TestCase(0, FeeType.Percentage)]
    public void CalculateInitialFees_WithZeroForProcessingFee_ShouldThrowException(decimal processingFee, FeeType processingFeeType)
    {
        // Arrange
        int loanAmount = 10000;
        decimal? applicationFee = 100m;
        FeeType applicationFeeType = FeeType.Currency;
        decimal? otherFees = 50m;
        FeeType otherFeesType = FeeType.Currency;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            this._creditService.CalculateInitialFees(loanAmount, applicationFee, applicationFeeType, processingFee, processingFeeType, otherFees, otherFeesType);
        }, "Processing fee must be greater than zero.");
    }

    [TestCase(0, FeeType.Currency)]
    [TestCase(0, FeeType.Percentage)]
    public void CalculateInitialFees_WithZeroForOtherFees_ShouldThrowException(decimal otherFees, FeeType otherFeesType)
    {
        // Arrange
        int loanAmount = 10000;
        decimal? applicationFee = 100m;
        FeeType applicationFeeType = FeeType.Currency;
        decimal? processingFee = 5m;
        FeeType processingFeeType = FeeType.Percentage;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            this._creditService.CalculateInitialFees(loanAmount, applicationFee, applicationFeeType, processingFee, processingFeeType, otherFees, otherFeesType);
        }, "Other fees must be greater than zero.");
    }
}
