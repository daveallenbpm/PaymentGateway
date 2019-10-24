using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Validation
{
    public class NonNegativeValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal)
            {
                var d = (decimal)value;

                if (d < 0)
                {
                    return new ValidationResult($"{validationContext.DisplayName} must not be negative.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"This data type is not compatable with this attribute.");
        }
    }
}