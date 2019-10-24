using PaymentGateway.Controllers;
using Xunit;

namespace PaymentGateway.Test.Unit
{
    public class CardHelpersTests
    {
        [Theory]
        [InlineData("1234567812345678", "************5678")]
        [InlineData("5555555555554444", "************4444")]
        [InlineData("8080678734341414", "************1414")]
        public void MaskCardNumberShouldCorrectlyMaskTheCardNumber(string cardNumber, string expectedMaskedCardNumber)
        {
            Assert.Equal(expectedMaskedCardNumber, CardHelpers.MaskCardNumber(cardNumber));
        }
    }
}
