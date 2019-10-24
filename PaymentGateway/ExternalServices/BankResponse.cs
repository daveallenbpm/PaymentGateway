using System;

namespace PaymentGateway.ExternalServices
{
    public class BankResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}