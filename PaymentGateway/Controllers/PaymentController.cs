using Microsoft.AspNetCore.Mvc;
using PaymentGateway.DataAccess;
using PaymentGateway.ExternalServices;
using PaymentGateway.Models;
using System;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly IBank _bank;
        private readonly IPaymentRepository _payments;

        public PaymentController(IBank bank, IPaymentRepository payments)
        {
            _bank = bank;
            _payments = payments;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var paymentEntity = _payments.Get(id);

            if (paymentEntity == null)
            {
                return NotFound();
            }

            PaymentGetDto paymentDto = MapToPaymentGetDto(paymentEntity);

            return new JsonResult(paymentDto);
        }

        [HttpGet("paymentid/{paymentId}")]
        public IActionResult GetByPaymentId(Guid paymentId)
        {
            var paymentEntity = _payments.GetByPaymentId(paymentId);

            if (paymentEntity == null)
            {
                return NotFound();
            }

            PaymentGetDto paymentDto = MapToPaymentGetDto(paymentEntity);

            return new JsonResult(paymentDto);
        }

        [HttpPost]
        public IActionResult Process([FromBody] PaymentPostDto payment)
        {
            var result = _bank.RequestPayment(payment);

            var paymentEntity = new PaymentEntity
            {
                PaymentId = result.PaymentId,
                PaymentStatus = result.Status,
                CardNumber = payment.CardNumber,
                ExpiryDate = payment.ExpiryDate.Date,
                Amount = payment.Amount,
                Currency = payment.Currency,
                CVV = payment.CVV
            };

            _payments.Add(paymentEntity);

            return new JsonResult(result);
        }

        private static PaymentGetDto MapToPaymentGetDto(PaymentEntity paymentEntity)
        {
            return new PaymentGetDto(paymentEntity.CardNumber)
            {
                Id = paymentEntity.Id,
                PaymentId = paymentEntity.PaymentId,
                PaymentStatus = paymentEntity.PaymentStatus,
                ExpiryDate = paymentEntity.ExpiryDate,
                Amount = paymentEntity.Amount,
                Currency = paymentEntity.Currency,
                CVV = paymentEntity.CVV
            };
        }
    }
}