using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentGateway.Test.Unit
{
    public class TestHelpers
    {
        public static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        public static PaymentPostDto GenerateValidPaymentDto()
        {
            return new PaymentPostDto
            {
                CardNumber = "5555555555554444",
                ExpiryDate = DateTime.Now.AddDays(10),
                Amount = 10,
                Currency = "GBP",
                CVV = "123"
            };
        }
    }
}
