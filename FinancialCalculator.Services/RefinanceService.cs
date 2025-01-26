using FinancialCalculator.Common;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using static FinancialCalculator.Common.RefinanceConstraints;

namespace FinancialCalculator.Services;

public class RefinanceService : IRefinanceService
{
    private readonly IPaymentCalculationService _paymentCalculationService;
    private readonly IFeeCalculationService _feesCalculationService;
    
    public RefinanceService()
    {
        this._paymentCalculationService = new PaymentCalculationService();
        this._feesCalculationService = new FeeCalculationService();
    }
    
    /// <inheritdoc />
    public RefinanceResultDto Calculate(RefinanceServiceInputDto input)
    {
        // 1) Validate input
        if (input.LoanTermInMonths < MIN_LOAN_TERM_IN_MONTHS ||
            input.LoanTermInMonths > MAX_LOAN_TERM_IN_MONTHS)
        {
            throw new ArgumentException(ERROR_LOAN_TERM_IN_MONTHS);
        }
        
        if (input.LoanAmount.CompareTo(new BigDecimal(MIN_LOAN_AMOUNT)) < 0 ||
            input.LoanAmount.CompareTo(new BigDecimal(MAX_LOAN_AMOUNT)) > 0)
        {
            throw new ArgumentException(ERROR_LOAN_AMOUNT);
        }
        
        if (input.AnnualInterestRate.CompareTo(new BigDecimal(MIN_ANNUAL_INTEREST_RATE_AMOUNT)) < 0 ||
            input.AnnualInterestRate.CompareTo(new BigDecimal(MAX_ANNUAL_INTEREST_RATE_AMOUNT)) > 0)
        {
            throw new ArgumentException(ERROR_ANNUAL_INTEREST_RATE_AMOUNT);
        }
        
        if (input.NewAnnualInterestRate.CompareTo(new BigDecimal(MIN_NEW_ANNUAL_INTEREST_RATE_AMOUNT)) < 0 ||
            input.NewAnnualInterestRate.CompareTo(new BigDecimal(MAX_NEW_ANNUAL_INTEREST_RATE_AMOUNT)) > 0)
        {
            throw new ArgumentException(ERROR_NEW_ANNUAL_INTEREST_RATE_AMOUNT);
        }
        
        if (input.ContributionsMade < MIN_CONTRIBUTIONS_MADE)
        {
            throw new ArgumentException(ERROR_CONTRIBUTIONS_MADE);
        }
        
        if (input.ContributionsMade >= input.LoanTermInMonths)
        {
            throw new ArgumentException(ERROR_CONTRIBUTIONS_MADE_TOO_HIGH);
        }
        
        if (input.EarlyRepaymentFee.CompareTo(new BigDecimal(MIN_EARLY_REPAYMENT_FEE)) < 0 ||
            input.EarlyRepaymentFee.CompareTo(new BigDecimal(MAX_EARLY_REPAYMENT_FEE)) > 0)
        {
            throw new ArgumentException(ERROR_EARLY_REPAYMENT_FEE);
        }
        
        if (input.InitialFee.CompareTo(new BigDecimal(MIN_INITIAL_FEE)) < 0 ||
            input.InitialFee.CompareTo(new BigDecimal(MAX_INITIAL_FEE)) > 0)
        {
            throw new ArgumentException(ERROR_INITIAL_FEE);
        }
        
        // 2) "Current Credit" scenario
        // (a) remaining principal after X contributions
        BigDecimal remainingPrincipal = this._paymentCalculationService.CalculateRemainingRefinanceBalance(
            input.LoanAmount,
            input.AnnualInterestRate,
            input.LoanTermInMonths,
            input.ContributionsMade
        );
        
        // (b) early repayment fee if we close it now
        BigDecimal currentEarlyRepaymentFee = this._feesCalculationService.CalculateEarlyRepaymentFee(
            remainingPrincipal,
            input.EarlyRepaymentFee
        );
        
        // (c) how many months remain on the current loan
        int currentRemainingMonths = input.LoanTermInMonths - input.ContributionsMade;
        if (currentRemainingMonths < 0)
        {
            currentRemainingMonths = 0;
        }
        
        // (d) monthly payment for the remainder of the current loan
        BigDecimal currentMonthlyPayment = this._paymentCalculationService.CalculateMonthlyPayment(
            remainingPrincipal,
            input.AnnualInterestRate,
            currentRemainingMonths
        );
        
        // (e) total cost if you keep paying the old loan 
        BigDecimal currentTotalPaid = currentMonthlyPayment * new BigDecimal(currentRemainingMonths);

        // 3) "New Loan" scenario
        // (a) the new loan principal is typically the "remainingPrincipal"
        BigDecimal newLoanPrincipal = remainingPrincipal;
        
        // (b) new loan initial fees
        BigDecimal newInitialFees = this._feesCalculationService.CalculateInitialFees(
            newLoanPrincipal,
            input.InitialFee,
            input.InitialFeeCurrency
        );
        
        // (c) new loan term
        int newLoanTermInMonths = currentRemainingMonths;
        if (newLoanTermInMonths < 1) 
        {
            newLoanTermInMonths = 1; 
        }
        
        // (d) new monthly payment
        BigDecimal newMonthlyPayment = this._paymentCalculationService.CalculateMonthlyPayment(
            newLoanPrincipal,
            input.NewAnnualInterestRate,
            newLoanTermInMonths
        );
        
        // (e) total cost of the new loan = monthlyPayment * newLoanTermInMonths + any initial fees
        BigDecimal newLoanInstallmentsSum = newMonthlyPayment * new BigDecimal(newLoanTermInMonths);
        BigDecimal newTotalPaid = newLoanInstallmentsSum + newInitialFees + currentEarlyRepaymentFee;
        
        // 4) Compare and create message
        BigDecimal difference = currentTotalPaid - newTotalPaid;
        
        // Build a user-friendly message
        string msg = difference > BigDecimal.Zero 
            ? $"Refinancing is beneficial. You save {difference.BankersRounding(2)} BGN." 
            : "Refinancing is NOT beneficial. The cost of refinancing is higher than the expected savings.";
        
        // 5) Return the result in a data structure
        return new RefinanceResultDto
        {
            CurrentMonthlyInstallment  = currentMonthlyPayment,
            CurrentTotalPaid           = currentTotalPaid,
            CurrentEarlyRepaymentFee   = currentEarlyRepaymentFee,

            NewMonthlyInstallment      = newMonthlyPayment,
            NewTotalPaid               = newTotalPaid,
            NewInitialFees             = newInitialFees,

            SavingsDifference          = difference,
            Message                    = msg
        };
    }
}
