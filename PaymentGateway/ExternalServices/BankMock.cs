using System;
using PaymentGateway.Enums;
using PaymentGateway.Models;

namespace PaymentGateway.ExternalServices
{
    public class BankMock : IBank
    {
        public BankResponse RequestPayment(PaymentPostDto payment)
        {
            if (payment == null)
                throw new ArgumentNullException("payment");

            Guid paymentId = Guid.NewGuid();

            if (payment.ExpiryDate.Date < System.DateTime.Now.Date)
            {
                return new BankResponse
                {
                    PaymentId = paymentId,
                    Status = PaymentStatus.Rejected.ToString()
                };
            }

            return new BankResponse
            {
                PaymentId = paymentId,
                Status = PaymentStatus.Success.ToString()
            };
        }
    }
}