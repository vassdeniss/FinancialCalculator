using System.ComponentModel.DataAnnotations;
using FinancialCalculator.Common;

namespace FinancialCalculator.Web.Attribute;

public class BigDecimalRangeAttribute(int min, int max) : ValidationAttribute
{
    private readonly BigDecimal _min = new(min);
    private readonly BigDecimal _max = new(max);
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not BigDecimal bigDecimalValue)
        {
            return ValidationResult.Success;
        }

        if (bigDecimalValue < this._min || bigDecimalValue > this._max)
        {
            return new ValidationResult(this.ErrorMessage ?? $"Value must be between {this._min} and {this._max}.");
        }

        return ValidationResult.Success;
    }
}
