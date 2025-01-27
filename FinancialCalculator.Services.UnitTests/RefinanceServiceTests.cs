using FinancialCalculator.Common;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.UnitTests
{
    [TestFixture]
    public class RefinanceServiceTests
    {
        private RefinanceService service;
        private RefinanceServiceInputDto defaultInput;

        [SetUp]
        public void Setup()
        {
            service = new RefinanceService();
            defaultInput = CreateDefaultRefinanceServiceInputDto();
        }

        private RefinanceServiceInputDto CreateDefaultRefinanceServiceInputDto()
        {
            return new RefinanceServiceInputDto
            {
                LoanTermInMonths = 24,
                LoanAmount = BigDecimal.Parse("1000"),
                AnnualInterestRate = BigDecimal.Parse("5"),
                ContributionsMade = 12,
                EarlyRepaymentFee = BigDecimal.Parse("1"),
                InitialFee = BigDecimal.Parse("0.1"),
                NewAnnualInterestRate = BigDecimal.Parse("4")
            };
        }

        [Test]
        [TestCase(0, Description = "Loan term in months below minimum")]
        [TestCase(1000, Description = "Loan term in months above maximum")]
        public void Calculate_LoanTermInMonthsOutOfRange_ThrowsArgumentException(int loanTermInMonths)
        {
            // Arrange
            defaultInput.LoanTermInMonths = loanTermInMonths;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Loan month must be between 1 and 999."));
        }

        [Test]
        [TestCase("0", Description = "Loan amount below minimum")]
        [TestCase("1000000000", Description = "Loan amount above maximum")]
        public void Calculate_LoanAmountOutOfRange_ThrowsArgumentException(string loanAmountStr)
        {
            // Arrange
            defaultInput.LoanAmount = BigDecimal.Parse(loanAmountStr);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Loan amount must be between 1 and 999999999 BGN."));
        }

        [Test]
        [TestCase("0", Description = "Annual interest rate below minimum")]
        [TestCase("100", Description = "Annual interest rate above maximum")]
        public void Calculate_AnnualInterestRateOutOfRange_ThrowsArgumentException(string annualInterestRateStr)
        {
            // Arrange
            defaultInput.AnnualInterestRate = BigDecimal.Parse(annualInterestRateStr);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Annual interest rate must be between 1 and 99%."));
        }

        [Test]
        [TestCase(-1, Description = "Contributions made below minimum")]
        public void Calculate_ContributionsMadeOutOfRange_ThrowsArgumentException(int contributionsMade)
        {
            // Arrange
            defaultInput.ContributionsMade = contributionsMade;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Need to have at least one contribution."));
        }

        [Test]
        [TestCase(25, 24, Description = "Contributions made greater than loan term in months")]
        [TestCase(24, 24, Description = "Contributions made equal to loan term in months")]
        public void Calculate_ContributionsMadeTooHigh_ThrowsArgumentException(int contributionsMade, int loanTermInMonths)
        {
            // Arrange
            defaultInput.ContributionsMade = contributionsMade;
            defaultInput.LoanTermInMonths = loanTermInMonths;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Contributions must be lower than the loan term in months."));
        }

        [Test]
        [TestCase("-1", Description = "Early repayment fee below minimum")]
        [TestCase("100", Description = "Early repayment fee above maximum")]
        public void Calculate_EarlyRepaymentFeeOutOfRange_ThrowsArgumentException(string earlyRepaymentFeeStr)
        {
            // Arrange
            defaultInput.EarlyRepaymentFee = BigDecimal.Parse(earlyRepaymentFeeStr);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Early repayment fee must be between 0 and 99%."));
        }

        [Test]
        [TestCase("-1", Description = "Initial fee below minimum")]
        [TestCase("1000000000", Description = "Initial fee above maximum")]
        public void Calculate_InitialFeeOutOfRange_ThrowsArgumentException(string initialFeeStr)
        {
            // Arrange
            defaultInput.InitialFee = BigDecimal.Parse(initialFeeStr);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Initial fee must be between 0 and 999999999."));
        }

        [Test]
        [TestCase("0", Description = "New annual interest rate below minimum")]
        [TestCase("100", Description = "New annual interest rate above maximum")]
        public void Calculate_NewAnnualInterestRateOutOfRange_ThrowsArgumentException(string newAnnualInterestRateStr)
        {
            // Arrange
            defaultInput.NewAnnualInterestRate = BigDecimal.Parse(newAnnualInterestRateStr);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => service.Calculate(defaultInput));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("New annual interest rate must be between 1 and 99%."));
        }

        [Test]
        public void Calculate_RefinancingNotBeneficial_GeneratesIncorrectMessage()
        {
            // Arrange
            defaultInput.LoanAmount = BigDecimal.Parse("1000");
            defaultInput.AnnualInterestRate = BigDecimal.Parse("5");
            defaultInput.NewAnnualInterestRate = BigDecimal.Parse("6");

            // Act
            RefinanceResultDto result = service.Calculate(defaultInput);

            // Assert
            Assert.That(result.Message, Is.EqualTo("Refinancing is NOT beneficial. The cost of refinancing is higher than the expected savings."));
        }

        [Test]
        [TestCase("100", 12, "2", 10, "0", "0.1", "0", "0", "0.04", Description = "Refinancing beneficial with specific data from the image")]
        public void Calculate_RefinancingBeneficial_GeneratesCorrectMessage(string loanAmountStr, int loanTermInMonths, string annualInterestRateStr, int contributionsMade, string earlyRepaymentFeeStr, string newAnnualInterestRateStr, string initialFeePercentStr, string initialFeeCurrencyStr, string expectedSavingsStr)
        {
            // Arrange
            defaultInput.LoanAmount = BigDecimal.Parse(loanAmountStr);
            defaultInput.LoanTermInMonths = loanTermInMonths;
            defaultInput.AnnualInterestRate = BigDecimal.Parse(annualInterestRateStr);
            defaultInput.ContributionsMade = contributionsMade;
            defaultInput.EarlyRepaymentFee = BigDecimal.Parse(earlyRepaymentFeeStr);
            defaultInput.NewAnnualInterestRate = BigDecimal.Parse(newAnnualInterestRateStr);
            defaultInput.InitialFee = BigDecimal.Parse(initialFeePercentStr);
            defaultInput.InitialFeeCurrency = BigDecimal.Parse(initialFeeCurrencyStr);

            // Act
            RefinanceResultDto result = service.Calculate(defaultInput);

            // Assert
            Assert.That(result.Message, Is.EqualTo($"Refinancing is beneficial. You save {expectedSavingsStr} BGN."));
        }

        [Test]
        [TestCase("999999999", 999, "99", 998, "99", "99", "999999999", "999999999", "82499999.92", "82499999.92", "989999999.01", "1082499998.92", "10000003052500000", "10000000980000000", "999999999", "Refinancing is NOT beneficial. The cost of refinancing is higher than the expected savings.",
        Description = "Validates that the Calculate method returns correct values")]
        public void Calculate_ValidInputs_ReturnsCorrectResult(string loanAmountStr, int loanTermInMonths, string annualInterestRateStr, int contributionsMade, string earlyRepaymentFeeStr, string newAnnualInterestRateStr, string initialFeePercentStr, string initialFeeCurrencyStr,
    string expectedCurrentMonthlyInstallmentStr, string expectedCurrentTotalPaidStr, string expectedCurrentEarlyRepaymentFeeStr, string expectedNewMonthlyInstallmentStr, string expectedNewTotalPaidStr, string expectedNewInitialFeesStr, string expectedSavingsDifferenceStr, string expectedMessage)
        {
            // Arrange
            defaultInput.LoanAmount = BigDecimal.Parse(loanAmountStr);
            defaultInput.LoanTermInMonths = loanTermInMonths;
            defaultInput.AnnualInterestRate = BigDecimal.Parse(annualInterestRateStr);
            defaultInput.ContributionsMade = contributionsMade;
            defaultInput.EarlyRepaymentFee = BigDecimal.Parse(earlyRepaymentFeeStr);
            defaultInput.NewAnnualInterestRate = BigDecimal.Parse(newAnnualInterestRateStr);
            defaultInput.InitialFee = BigDecimal.Parse(initialFeePercentStr);
            defaultInput.InitialFeeCurrency = BigDecimal.Parse(initialFeeCurrencyStr);

            BigDecimal expectedCurrentMonthlyInstallment = BigDecimal.Parse(expectedCurrentMonthlyInstallmentStr);
            BigDecimal expectedCurrentTotalPaid = BigDecimal.Parse(expectedCurrentTotalPaidStr);
            BigDecimal expectedCurrentEarlyRepaymentFee = BigDecimal.Parse(expectedCurrentEarlyRepaymentFeeStr);
            BigDecimal expectedNewMonthlyInstallment = BigDecimal.Parse(expectedNewMonthlyInstallmentStr);
            BigDecimal expectedNewTotalPaid = BigDecimal.Parse(expectedNewTotalPaidStr);
            BigDecimal expectedNewInitialFees = BigDecimal.Parse(expectedNewInitialFeesStr);
            BigDecimal expectedSavingsDifference = BigDecimal.Parse(expectedSavingsDifferenceStr);

            // Act
            RefinanceResultDto result = service.Calculate(defaultInput);

            // Assert
            Assert.That(result.CurrentMonthlyInstallment.BankersRounding(2), Is.EqualTo(expectedCurrentMonthlyInstallment));
            Assert.That(result.CurrentTotalPaid.BankersRounding(2), Is.EqualTo(expectedCurrentTotalPaid));
            Assert.That(result.CurrentEarlyRepaymentFee.BankersRounding(2), Is.EqualTo(expectedCurrentEarlyRepaymentFee));
            Assert.That(result.NewMonthlyInstallment.BankersRounding(2), Is.EqualTo(expectedNewMonthlyInstallment));
            Assert.That(result.NewTotalPaid.BankersRounding(2), Is.EqualTo(expectedNewTotalPaid));
            Assert.That(result.NewInitialFees.BankersRounding(2), Is.EqualTo(expectedNewInitialFees));
            Assert.That(result.SavingsDifference.BankersRounding(2), Is.EqualTo(expectedSavingsDifference));
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }
}
