using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Validation;

namespace PaymentGateway.Models
{
    public class PaymentPostDto
    {
        [CreditCard]
        [Required]
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        [NonNegativeValue]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression("[0-9]+$")]
        public string CVV { get; set; }
    }
}