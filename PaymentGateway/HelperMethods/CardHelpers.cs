using System.Text;

namespace PaymentGateway.Controllers
{
    public static class CardHelpers
    {
        public static string MaskCardNumber(string cardNumber)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 12; i++)
            {
                builder.Append('*');
            }

            builder.Append(cardNumber.Substring(12));

            return builder.ToString();
        }
    }
}