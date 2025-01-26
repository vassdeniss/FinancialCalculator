using FinancialCalculator.Common;
using FinancialCalculator.Services.Contracts;
using FinancialCalculator.Services.DTO;
using static FinancialCalculator.Common.LeaseConstraints;

namespace FinancialCalculator.Services;

public class LeaseService : ILeaseService
{
    /// <inheritdoc />
    public LeaseResultDto CalculateLeaseResult(LeaseServiceInputDto input)
    {
        if (input.Price.CompareTo(new BigDecimal(MIN_PRICE)) < 0 
            || input.Price.CompareTo(new BigDecimal(MAX_PRICE)) > 0)
        {
            throw new ArgumentException(ERROR_PRICE);
        }

        if (input.InitialPayment.CompareTo(new BigDecimal(MIN_INITIAL_PAYMENT)) < 0)
        {
            throw new ArgumentException(ERROR_INITIAL_PAYMENT);
        }
        if (input.InitialPayment >= input.Price)
        {
            throw new ArgumentException(ERROR_INITIAL_PAYMENT_AGAINST_PRICE);
        }

        if (input.LeaseTermInMonths < MIN_LEASE_TERM_IN_MONTHS
            || input.LeaseTermInMonths > MAX_LEASE_TERM_IN_MONTHS)
        {
            throw new ArgumentException(ERROR_LEASE_TERM_IN_MONTHS);
        }

        if (input.MonthlyInstallment.CompareTo(new BigDecimal(MIN_MONTHLY_INSTALLMENT)) < 0
            || input.MonthlyInstallment.CompareTo(new BigDecimal(MAX_MONTHLY_INSTALLMENT)) > 0)
        {
            throw new ArgumentException(ERROR_MONTHLY_INSTALLMENT);
        }
        
        // Assume total paid is initial payment + monthly installments over the term + any initial fee
        BigDecimal totalPaid = input.InitialPayment + input.MonthlyInstallment * new BigDecimal(input.LeaseTermInMonths) + input.InitialProcessingFee;

        // Total fees should be the percentage of InitialProcessingFee from the Price

        BigDecimal totalFees = totalPaid - input.Price;

        // QUESTION: Why do we have this check if we already validate that initial payment cannot be greater than or equal to the price?

        // Determine how much is actually financed.
        BigDecimal financedAmount = input.Price - input.InitialPayment;
        if (financedAmount < BigDecimal.Zero)
        {
            return new LeaseResultDto
            {
                AnnualCostPercent = BigDecimal.Zero,
                TotalPaid = totalPaid,
                TotalFees = totalFees
            };
        }
        
        BigDecimal annualCostPercent = this.CalculateApr(
            financedAmount,
            input.MonthlyInstallment,
            input.InitialProcessingFee,
            input.LeaseTermInMonths);

        return new LeaseResultDto
        {
            AnnualCostPercent = annualCostPercent,
            TotalPaid = totalPaid,
            TotalFees = totalFees,
        };
    }
    
    /// <summary>
    /// Uses a simple binary-search approach to find the monthly IRR 
    /// that sets the Net Present Value (NPV) = 0.
    /// </summary>
    public BigDecimal CalculateApr(
        BigDecimal financedAmount,
        BigDecimal monthlyInstallment,
        BigDecimal initialFee,
        int leaseTermInMonths)
    {
        // NPV function:
        //   at t=0: +financedAmount (inflow) - initialFee (outflow)
        //   at t=1..n: -monthlyInstallment for each month
        // We solve for r in Σ (cashflow / (1+r)^t ) = 0
        // Return that monthly rate.
        Func<BigDecimal, BigDecimal> npvFunc = (rate) =>
        {
            BigDecimal npv = financedAmount - initialFee; // net inflow at t=0
            for (int t = 1; t <= leaseTermInMonths; t++)
            {
                // discount each monthly installment
                npv -= monthlyInstallment / (BigDecimal.One + rate).Power(t);
            }
            return npv;
        };
        
        BigDecimal lower = BigDecimal.Zero;
        BigDecimal upper = BigDecimal.One;
        for (int i = 0; i < 100; i++)
        {
            BigDecimal mid = (lower + upper) / new BigDecimal(2);
            BigDecimal npv = npvFunc(mid);
            
            if (npv > BigDecimal.Zero)
                upper = mid;   // need higher rate
            else
                lower = mid;   // need lower rate
        }
        
        // 3. Final approximate monthly IRR is midpoint
        BigDecimal monthlyIrr = (lower + upper) / new BigDecimal(2);

        // 4. Convert monthly IRR -> APR = ((1 + r)^12 - 1) * 100
        BigDecimal apr = ((BigDecimal.One + monthlyIrr).Power(12) - BigDecimal.One)
                         * new BigDecimal(100);
        
        return apr;
    }
}
