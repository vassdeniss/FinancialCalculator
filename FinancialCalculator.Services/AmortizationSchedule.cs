using FinancialCalculator.Common;
using FinancialCalculator.Common.Enums;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services;

class AmortizationSchedule : IAmortizationSchedule
{
    private readonly IFeeCalculationService _feeCalculationService;

    public AmortizationSchedule()
    {
        this._feeCalculationService = new FeeCalculationService();
    }
    
    /// <inheritdoc />
    public CreditResultDto GenerateAmortizationSchedule(
        decimal loanAmount,
        int loanTermInMonths,
        int gracePeriodMonths,
        int remainingPromotionalMonths,
        double annualPromotionalInterestRate,
        double annualInterestRate,
        decimal monthlyPaymentPromo,
        decimal monthlyPaymentNormal,
        CreditInputDto input,
        decimal totalInitialFees)
    {
        List<AmortizationEntry> schedule =
        [
            new()
            {
                Month = 0,
                Date = DateTime.Today.ToString("dd.MM.yyyy"),
                RemainingBalance = loanAmount,
                Payment = 0m,
                Principal = 0m,
                Interest = 0m,
                Fees = totalInitialFees
            }
        ];

        decimal balance = loanAmount;
        decimal totalPayments = 0m;
        decimal totalInterest = 0m;
        decimal totalPrincipal = 0m;
        decimal totalFees = totalInitialFees; // Start with initial fees

        decimal monthlyInterestRatePromo = (decimal)(annualPromotionalInterestRate / 100 / 12);
        decimal monthlyInterestRateNormal = (decimal)(annualInterestRate / 100 / 12);
        
        decimal fixedPrincipalPayment = input.PaymentType == PaymentType.Decreasing 
            ? balance / (loanTermInMonths - gracePeriodMonths) 
            : 0m;
        
        for (int month = 1; month <= loanTermInMonths; month++)
        {
            decimal interestRate = 0m;
            decimal monthlyPayment = 0m;
            decimal interestPayment = 0m;
            decimal principalPayment = 0m;
            decimal fees = 0.0m;

            decimal tempBalance = balance; // For fee calculations

            if (month <= gracePeriodMonths)
            {
                // Grace Period: Interest-only payments
                interestRate = month <= input.PromotionalPeriodMonths 
                    ? monthlyInterestRatePromo 
                    : monthlyInterestRateNormal;

                interestPayment = balance * interestRate;
                principalPayment = 0m;
                monthlyPayment = interestPayment;
            }
            else
            {
                // Repayment Period
                int monthsSinceGrace = month - gracePeriodMonths;
                if (input.PaymentType == PaymentType.Annuity)
                {
                    if (monthsSinceGrace <= remainingPromotionalMonths)
                    {
                        // Promotional interest rate after grace period
                        interestRate = monthlyInterestRatePromo;
                        monthlyPayment = monthlyPaymentPromo;
                    }
                    else
                    {
                        // Normal interest rate after grace period
                        interestRate = monthlyInterestRateNormal;
                        monthlyPayment = monthlyPaymentNormal;
                    }

                    interestPayment = balance * interestRate;
                    principalPayment = monthlyPayment - interestPayment;

                    // Ensure balance doesn't go negative
                    if (balance - principalPayment < 0m)
                    {
                        principalPayment = balance;
                        monthlyPayment = principalPayment + interestPayment;
                    }
                }
                else if (input.PaymentType == PaymentType.Decreasing)
                {
                    // Decreasing Payment Calculation
                    principalPayment = fixedPrincipalPayment;

                    if (monthsSinceGrace <= remainingPromotionalMonths)
                    {
                        interestRate = monthlyInterestRatePromo;
                    }
                    else
                    {
                        interestRate = monthlyInterestRateNormal;
                    }

                    interestPayment = balance * interestRate;
                    monthlyPayment = principalPayment + interestPayment;

                    // Adjust principal payment for the last payment
                    if (balance < principalPayment)
                    {
                        principalPayment = balance;
                        monthlyPayment = principalPayment + interestPayment;
                    }
                }
            }

            balance -= principalPayment;

            // Calculate fees
            fees += this._feeCalculationService.CalculateFee(input.MonthlyManagementFee, input.MonthlyManagementFeeType, tempBalance)
                   + this._feeCalculationService.CalculateFee(input.OtherMonthlyFees, input.OtherMonthlyFeesType, tempBalance);
            if (month - 1 > 0 && (month - 1) % 12 == 0)
            {
                fees += this._feeCalculationService.CalculateFee(input.AnnualManagementFee, input.AnnualManagementFeeType, tempBalance)
                        + this._feeCalculationService.CalculateFee(input.OtherAnnualFees, input.OtherAnnualFeesType, tempBalance);
            }
            
            totalPayments += monthlyPayment;
            totalInterest += interestPayment;
            totalPrincipal += principalPayment;
            totalFees += fees;

            schedule.Add(new AmortizationEntry
            {
                Month = month,
                Date = DateTime.Today.AddMonths(month).ToString("dd.MM.yyyy"),
                Payment = monthlyPayment,
                Principal = principalPayment,
                Interest = interestPayment,
                Fees = fees,
                RemainingBalance = tempBalance
            });
        }

        decimal averageMonthlyPayment = totalPayments / loanTermInMonths;
        decimal apr = this.CalculateApr(input.LoanAmount, totalInitialFees, schedule);

        return new CreditResultDto
        {
            TotalPayments = totalPayments + totalFees,
            TotalInterest = totalInterest,
            TotalPrincipal = totalPrincipal,
            TotalInitialFees = totalInitialFees,
            TotalFees = totalFees,
            AverageMonthlyPayment = averageMonthlyPayment,
            AmortizationSchedule = schedule,
            Apr = apr
        };
    }
    
    /// <summary>
    /// Calculates the Annual Percentage Rate (APR) of a loan based on the loan amount, initial fees, and the amortization schedule.
    /// </summary>
    /// <param name="loanAmount">
    /// The principal amount of the loan.
    /// </param>
    /// <param name="totalInitialFees">The total initial fees paid upfront.</param>
    /// <param name="schedule">
    /// A list of <see cref="AmortizationEntry"/> representing the loan's payment schedule.
    /// </param>
    /// <returns>
    /// The calculated APR as a <c>decimal</c> percentage (e.g., 5.5 for 5.5%).
    /// </returns>
    /// <remarks>
    /// <para>
    /// The APR is calculated by finding the Internal Rate of Return (IRR) for the loan's cash flows, which include the initial loan amount and all subsequent payments.
    /// The IRR is the discount rate that makes the Net Present Value (NPV) of all cash flows equal to zero.
    /// </para>
    /// <para>
    /// Net Present Value (NPV) Formula:
    /// </para>
    /// <para>
    ///            N       M_t
    /// NPV = -P + Σ   ----------
    ///           i=1  (1 + r)^t
    /// </para>
    /// <para>
    /// Where:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>P = Loan amount (principal)</description>
    ///   </item>
    ///   <item>
    ///     <description>M_t = Monthly payment at time t</description>
    ///   </item>
    ///   <item>
    ///     <description>r = Monthly discount rate (what we're solving for)</description>
    ///   </item>
    ///   <item>
    ///     <description>N = Total number of payments</description>
    ///   </item>
    /// </list>
    /// <para>
    /// The method uses the bisection method, a numerical technique, to find the rate r that sets the NPV to zero.
    /// Once the monthly rate is found, it's annualized by multiplying by 12 and converted to a percentage to get
    /// the APR.
    /// </para>
    /// </remarks>
    private decimal CalculateApr(decimal loanAmount, decimal totalInitialFees, List<AmortizationEntry> schedule)
    {
        // Define the function for Net Present Value
        Func<decimal, decimal> npvFunc = rate =>
        {
            // Net loan amount received is loanAmount - totalInitialFees
            decimal netLoanAmountReceived = loanAmount - totalInitialFees;

            // Start with the negative net loan amount received
            decimal npv = -netLoanAmountReceived;

            // Sum the present value of each payment
            for (int i = 0; i < schedule.Count; i++)
            {
                decimal payment = schedule[i].Payment + schedule[i].Fees;
                decimal t = i + 1; // Time period in months
                // Discount each payment back to present value
                npv += payment / (decimal)Math.Pow((double)(1 + rate), (double)t);
            }

            return npv;
        };

        // Use the bisection method to find the rate that makes NPV zero
        decimal lowerBound = 0.0m; // Lower bound of the interest rate
        decimal upperBound = 1.0m; // Upper bound (100% monthly rate is unrealistically high)
        decimal tolerance = 1e-9m; // Desired precision
        decimal internalRateOfReturn = 0.0m; // Internal Rate of Return (monthly rate)

        while (upperBound - lowerBound > tolerance)
        {
            decimal rate = (lowerBound + upperBound) / 2; // Midpoint of the interval
            decimal npv = npvFunc(rate); // Calculate NPV at this rate
            if (npv > 0)
                lowerBound = rate; // NPV positive, rate is too low
            else
                upperBound = rate; // NPV negative, rate is too high
        }

        // The IRR is the rate where NPV is zero
        internalRateOfReturn = (lowerBound + upperBound) / 2;

        // Convert monthly rate to annual percentage rate using compound interest
        decimal apr = ((decimal)Math.Pow((double)(1 + internalRateOfReturn), 12) - 1) * 100;

        return apr;
    }
}
