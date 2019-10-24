using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.DataAccess
{
    public class PaymentRepositoryMock : IPaymentRepository
    {
        private readonly List<PaymentEntity> payments;

        public PaymentRepositoryMock()
        {
            payments = new List<PaymentEntity>();
        }

        public void Add(PaymentEntity payment)
        {
            payment.Id = payments.Count + 1;
            payments.Add(payment);
        }

        public PaymentEntity Get(int id)
        {
            return payments.SingleOrDefault(p => p.Id == id);
        }
    }
}