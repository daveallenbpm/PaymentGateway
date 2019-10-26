using System.Linq;
using Xunit;

namespace PaymentGateway.Test.Unit
{
    public class PaymentDtoModelValidationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData((string)null)]
        [InlineData("12341234123412345")]
        [InlineData("123412341234123")]
        public void PaymentDto_model_validation_should_pick_up_invalid_credit_card_numbers(string creditCardNumber)
        {
            var payment = TestHelpers.GenerateValidPaymentDto();
            payment.CardNumber = creditCardNumber;

            var validationResults = TestHelpers.ValidateModel(payment);
            Assert.Equal(1, validationResults.Count);
            Assert.Contains(nameof(payment.CardNumber), validationResults.Single().MemberNames);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        [InlineData(-10000000)]
        public void PaymentDto_model_validation_should_pick_up_negative_amounts(decimal amount)
        {
            var payment = TestHelpers.GenerateValidPaymentDto();
            payment.Amount = amount;

            var validationResults = TestHelpers.ValidateModel(payment);
            Assert.Equal(1, validationResults.Count);
            Assert.Contains(nameof(payment.Amount), validationResults.Single().ErrorMessage);
        }

        [Theory]
        [InlineData("POUNDS")]
        [InlineData("")]
        [InlineData((string)null)]
        [InlineData("GB")]
        [InlineData("XYZ")]
        [InlineData("ABC")]
        public void PaymentDto_model_validation_should_pick_up_invalid_currency_values(string currency)
        {
            var payment = TestHelpers.GenerateValidPaymentDto();
            payment.Currency = currency;

            var validationResults = TestHelpers.ValidateModel(payment);
            Assert.True(validationResults.Count > 0);
            Assert.Contains(nameof(payment.Currency), validationResults.First().ErrorMessage);
        }

        [Theory]
        [InlineData("12")]
        [InlineData("")]
        [InlineData((string)null)]
        [InlineData("GB")]
        [InlineData("12A")]
        [InlineData("#12")]
        [InlineData("1593")]
        public void PaymentDto_model_validation_should_pick_up_invalid_CVV_values(string cvv)
        {
            var payment = TestHelpers.GenerateValidPaymentDto();
            payment.CVV = cvv;

            var validationResults = TestHelpers.ValidateModel(payment);
            Assert.True(validationResults.Count > 0);
            Assert.Contains(nameof(payment.CVV), validationResults.First().ErrorMessage);
        }

        [Fact]
        public void PaymentDto_model_validation_should_return_valid_for_a_valid_model()
        {
            var payment = TestHelpers.GenerateValidPaymentDto();

            var validationResults = TestHelpers.ValidateModel(payment);

            Assert.True(validationResults.Count == 0);
        }
    }
}
