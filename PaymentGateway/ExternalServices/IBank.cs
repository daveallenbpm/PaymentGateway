using PaymentGateway.Models;

namespace PaymentGateway.ExternalServices
{
    public interface IBank
    {
        BankResponse RequestPayment(PaymentPostDto payment);
    }
}