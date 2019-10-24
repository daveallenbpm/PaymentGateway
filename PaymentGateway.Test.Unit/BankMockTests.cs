using PaymentGateway.Enums;
using PaymentGateway.ExternalServices;
using System;
using Xunit;

namespace PaymentGateway.Test.Unit
{
    public class BankMockTests
    {
        [Fact]
        public void RequestPayment_should_throw_if_payment_is_null()
        {
            var bank = new BankMock();
            Assert.Throws<ArgumentNullException>(() => bank.RequestPayment(null));
        }

        [Fact]
        public void RequestPayment_should_return_rejected_if_expiry_date_exceeded()
        {
            var bank = new BankMock();
            var payment = TestHelpers.GenerateValidPaymentDto();
            payment.ExpiryDate = DateTime.Now.AddDays(-10);
            var result = bank.RequestPayment(payment);

            Assert.Equal(PaymentStatus.Rejected.ToString(), result.Status);
        }

        [Fact]
        public void RequestPayment_should_return_success_with_valid_expiry_date()
        {
            var bank = new BankMock();
            var payment = TestHelpers.GenerateValidPaymentDto();
            var result = bank.RequestPayment(payment);

            Assert.Equal(PaymentStatus.Success.ToString(), result.Status);
        }
    }
}
