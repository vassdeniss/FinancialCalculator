namespace FinancialCalculator.Web.Models;

public class CreditResult
{
    public decimal TotalPayments { get; set; }
    
    public decimal TotalInterest { get; set; }
    
    public decimal TotalPrincipal { get; set; }
    
    public decimal AverageMonthlyPayment { get; set; }
    
    public decimal Apr { get; set; }
}
