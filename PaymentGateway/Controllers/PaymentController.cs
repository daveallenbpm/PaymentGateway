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

        /// <summary>
        /// Returns the payment with the given id.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        /// <response code="200">Returns the payment</response>
        /// <response code="404">If the payment is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PaymentGetDto))]
        [ProducesResponseType(404, Type = typeof(NotFoundResult))]
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

        /// <summary>
        /// Returns the payment with the given paymentId.
        /// </summary>
        /// <param name="paymentId">The payment id of the payment (guid given by the bank, e.g. CDF86033-57D6-4C88-BF60-061FEA6888F9).</param>
        /// <response code="200">Returns the payment</response>
        /// <response code="404">If the payment is not found</response>
        [HttpGet("paymentid/{paymentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(PaymentGetDto))]
        [ProducesResponseType(404, Type = typeof(NotFoundResult))]
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

        /// <summary>
        /// Processes a payment with the supplied data.
        /// </summary>
        /// <param name="payment">The payment data.</param>
        /// <response code="200">When the payment is processed successfully.</response>
        /// <response code="400">When payment data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(BankResponse))]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
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