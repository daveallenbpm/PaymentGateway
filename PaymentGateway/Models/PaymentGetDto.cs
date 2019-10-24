using System;
using PaymentGateway.Controllers;

namespace PaymentGateway.Models
{
    public class PaymentGetDto
    {
        private readonly string _cardNumber;

        public PaymentGetDto(string cardNumber)
        {
            _cardNumber = cardNumber;
        }

        public int Id { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string CardNumber { get => CardHelpers.MaskCardNumber(_cardNumber); }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }
    }
}