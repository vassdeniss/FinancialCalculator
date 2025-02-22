﻿using FinancialCalculator.Common;
using FinancialCalculator.Services.DTO;

namespace FinancialCalculator.Services.Contracts;

interface IAmortizationSchedule
{
    /// <summary>
    /// Generates the amortization schedule and calculates totals for payments, interest, and principal.
    /// </summary>
    /// <param name="loanAmount">The original loan amount.</param>
    /// <param name="loanTermInMonths">The total loan term in months.</param>
    /// <param name="gracePeriodMonths">The number of remaining months in the grace period.</param>
    /// <param name="remainingPromotionalMonths">The number of remaining months in the promotional period.</param>
    /// <param name="annualPromotionalInterestRate">The annual interest rate during the promotional period (as a percentage).</param>
    /// <param name="annualInterestRate">The annual interest rate after the promotional period (as a percentage).</param>
    /// <param name="monthlyPaymentPromo">The monthly payment during the promotional period.</param>
    /// <param name="monthlyPaymentNormal">The monthly payment after the promotional period.</param>
    /// <param name="serviceInput">An instance of <see cref="CreditServiceInputDto"/> containing all loan parameters and fees.</param>
    /// <param name="totalInitialFees">The total initial fees paid upfront.</param>
    /// <returns>A <see cref="CreditResultDto"/> containing the amortization results.</returns>
    CreditResultDto GenerateAmortizationSchedule(
        BigDecimal loanAmount,
        int loanTermInMonths,
        int gracePeriodMonths,
        int remainingPromotionalMonths,
        BigDecimal annualPromotionalInterestRate,
        BigDecimal annualInterestRate,
        BigDecimal monthlyPaymentPromo,
        BigDecimal monthlyPaymentNormal,
        CreditServiceInputDto serviceInput,
        BigDecimal totalInitialFees);
}
