namespace FinancialCalculator.Services.Contracts;

public interface ILeasingService
{
    // TODO: Research APR
    decimal CalculateAPR();
    
    /// <summary>
    /// Calculates the total amount paid over the lease term, including the initial payment, monthly payments, and processing fee.
    /// </summary>
    /// <param name="itemPrice">The price of the item including VAT.</param>
    /// <param name="initialPayment">The initial down payment.</param>
    /// <param name="monthlyPayment">The amount of each monthly payment.</param>
    /// <param name="leaseTermMonths">The term of the lease in months.</param>
    /// <param name="processingFeeRate">The processing fee rate as a percentage of the item price.</param>
    /// <returns>The total amount paid with all fees as a decimal value.</returns>
    decimal CalculateTotalPaid(
        decimal itemPrice, 
        decimal initialPayment, 
        decimal monthlyPayment,
        int leaseTermMonths, 
        decimal processingFeeRate);
    
    /// <summary>
    /// Calculates the processing fee based on the item price and processing fee rate.
    /// </summary>
    /// <param name="itemPrice">The price of the item including VAT.</param>
    /// <param name="processingFeeRate">The processing fee rate as a percentage of the item price.</param>
    /// <returns>The calculated processing fee as a decimal value.</returns>
    int CalculateProcessingFees(decimal itemPrice, decimal processingFeeRate);
}
