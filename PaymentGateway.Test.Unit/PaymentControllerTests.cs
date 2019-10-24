using System;
using Xunit;
using PaymentGateway.Controllers;
using Moq;
using PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.DataAccess;
using PaymentGateway.ExternalServices;
using PaymentGateway.Enums;

namespace PaymentGateway.Test.Unit
{
    public class PaymentControllerTests
    {
        [Fact]
        public void Process_method_should_request_for_the_bank_to_process_the_payment()
        {
            var bank = new Mock<IBank>();
            bank.Setup(b => b.RequestPayment(It.IsAny<PaymentPostDto>())).Returns(new BankResponse
            {
                PaymentId = Guid.NewGuid(),
                Status = PaymentStatus.Success.ToString()
            });

            var payments = Mock.Of<IPaymentRepository>();
            var controller = new PaymentController(bank.Object, payments);

            var payment = new PaymentPostDto
            {
                CardNumber = "5555555555554444",
                ExpiryDate = DateTime.Now.AddDays(10),
                Amount = 10,
                Currency = "GBP",
                CVV = "321"
            };

            controller.Process(payment);

            bank.Verify(b => b.RequestPayment(It.Is<PaymentPostDto>(p => 
                p.CardNumber.Equals(payment.CardNumber) &&
                p.ExpiryDate.Equals(payment.ExpiryDate) &&
                p.Amount.Equals(payment.Amount) &&
                p.Currency.Equals(payment.Currency) &&
                p.CVV.Equals(payment.CVV))
                )
            );
        }

        [Fact]
        public void Process_method_should_return_payment_data_when_payment_is_successful_in_json_result()
        {
            var bank = new Mock<IBank>();

            var bankResponse = new BankResponse
            {
                PaymentId = Guid.NewGuid(),
                Status = PaymentStatus.Success.ToString()
            };

            bank.Setup(b => b.RequestPayment(It.IsAny<PaymentPostDto>())).Returns(bankResponse);

            var payments = Mock.Of<IPaymentRepository>();
            var controller = new PaymentController(bank.Object, payments);

            var payment = new PaymentPostDto
            {
                CardNumber = "5555555555554444",
                ExpiryDate = DateTime.Now.AddDays(10),
                Amount = 10,
                Currency = "GBP",
                CVV = "321"
            };

            var result = controller.Process(payment);

            Assert.True(result is JsonResult);
            var jsonResult = result as JsonResult;

            Assert.True(jsonResult.Value is BankResponse);
            var bankResponseResult = jsonResult.Value as BankResponse;

            Assert.Equal(bankResponse.PaymentId, bankResponseResult.PaymentId);
            Assert.Equal(PaymentStatus.Success.ToString(), bankResponseResult.Status);
        }
    }
}
