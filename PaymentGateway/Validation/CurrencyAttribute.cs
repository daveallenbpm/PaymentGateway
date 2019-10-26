using PaymentGateway.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaymentGateway.Validation
{
    public class CurrencyAttribute : ValidationAttribute
    {
        private string[] validCurrencies;
        public CurrencyAttribute()
        {
            validCurrencies = Enum.GetNames(typeof(Currency));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string)
            {
                var currency = (string)value;

                if (!validCurrencies.Contains(currency))
                {
                    return new ValidationResult($"{validationContext.DisplayName} must be an existing, valid currency.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"This data type is not compatible with this attribute.");
        }
    }
}
