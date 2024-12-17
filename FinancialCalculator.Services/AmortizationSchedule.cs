using System.Numerics;
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
        BigDecimal loanAmount,
        int loanTermInMonths,
        int gracePeriodMonths,
        int remainingPromotionalMonths,
        BigDecimal annualPromotionalInterestRate,
        BigDecimal annualInterestRate,
        BigDecimal monthlyPaymentPromo,
        BigDecimal monthlyPaymentNormal,
        CreditInputDto input,
        BigDecimal totalInitialFees)
    {
        List<AmortizationEntry> schedule =
        [
            new()
            {
                Month = 0,
                Date = DateTime.Today.ToString("dd.MM.yyyy"),
                RemainingBalance = loanAmount,
                Payment = BigDecimal.Zero,
                Principal = BigDecimal.Zero,
                Interest = BigDecimal.Zero,
                Fees = totalInitialFees
            }
        ];

        BigDecimal balance = loanAmount;
        BigDecimal totalPayments = BigDecimal.Zero;
        BigDecimal totalInterest = BigDecimal.Zero;
        BigDecimal totalPrincipal = BigDecimal.Zero;
        BigDecimal totalFees = totalInitialFees; // Start with initial fees

        BigDecimal monthlyInterestRatePromo = annualPromotionalInterestRate / new BigDecimal(100) / new BigDecimal(12);
        BigDecimal monthlyInterestRateNormal = annualInterestRate / new BigDecimal(100) / new BigDecimal(12);
        
        BigDecimal fixedPrincipalPayment = input.PaymentType == PaymentType.Decreasing 
            ? balance / new BigDecimal(loanTermInMonths - gracePeriodMonths) 
            : BigDecimal.Zero;
        
        for (int month = 1; month <= loanTermInMonths; month++)
        {
            BigDecimal interestRate = BigDecimal.Zero;
            BigDecimal monthlyPayment = BigDecimal.Zero;
            BigDecimal interestPayment = BigDecimal.Zero;
            BigDecimal principalPayment = BigDecimal.Zero;
            BigDecimal fees = BigDecimal.Zero;

            BigDecimal tempBalance = balance; // For fee calculations

            if (month <= gracePeriodMonths)
            {
                // Grace Period: Interest-only payments
                interestRate = month <= input.PromotionalPeriodMonths 
                    ? monthlyInterestRatePromo 
                    : monthlyInterestRateNormal;

                interestPayment = balance * interestRate;
                principalPayment = BigDecimal.Zero;
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
                    if ((balance - principalPayment).CompareTo(BigDecimal.Zero) < 0)
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

        BigDecimal averageMonthlyPayment = totalPayments / new BigDecimal(loanTermInMonths);
        BigDecimal apr = this.CalculateApr(input.LoanAmount, totalInitialFees, schedule);

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
    /// The calculated APR as a <c>BigDecimal</c> percentage (e.g., 5.5 for 5.5%).
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
    private BigDecimal CalculateApr(BigDecimal loanAmount, BigDecimal totalInitialFees, List<AmortizationEntry> schedule)
    {
        // Define the function for Net Present Value
        Func<BigDecimal, BigDecimal> npvFunc = rate =>
        {
            // Net loan amount received is loanAmount - totalInitialFees
            BigDecimal netLoanAmountReceived = loanAmount - totalInitialFees;

            // Start with the negative net loan amount received
            BigDecimal npv = -netLoanAmountReceived;

            // Sum the present value of each payment
            for (int i = 0; i < schedule.Count; i++)
            {
                BigDecimal payment = schedule[i].Payment + schedule[i].Fees;
                int t = i + 1; // Time period in months
                // Discount each payment back to present value
                npv += payment / (BigDecimal.One + rate).Power(new BigInteger(t));
            }

            return npv;
        };

        // Use the bisection method to find the rate that makes NPV zero
        BigDecimal lowerBound = BigDecimal.Zero; // Lower bound of the interest rate
        BigDecimal upperBound = BigDecimal.One; // Upper bound (100% monthly rate is unrealistically high)
        BigDecimal tolerance = BigDecimal.Parse("0.000000001"); // Desired precision
        BigDecimal internalRateOfReturn = BigDecimal.Zero; // Internal Rate of Return (monthly rate)

        while (upperBound - lowerBound > tolerance)
        {
            BigDecimal rate = (lowerBound + upperBound) / BigDecimal.Two; // Midpoint of the interval
            BigDecimal npv = npvFunc(rate); // Calculate NPV at this rate
            if (npv > BigDecimal.Zero)
                lowerBound = rate; // NPV positive, rate is too low
            else
                upperBound = rate; // NPV negative, rate is too high
        }

        // The IRR is the rate where NPV is zero
        internalRateOfReturn = (lowerBound + upperBound) / BigDecimal.Two;

        // Convert monthly rate to annual percentage rate using compound interest
        BigDecimal apr = ((BigDecimal.One + internalRateOfReturn).Power(new BigInteger(12)) - BigDecimal.One) * new BigDecimal(100);

        return apr;
    }
}
