using System;

namespace PaymentGateway.Test.Integration
{
    public class PaymentGetDto
    {
        public int Id { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }
    }
}