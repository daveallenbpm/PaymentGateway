using System.Linq;

namespace PaymentGateway.DataAccess
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(PaymentEntity payment)
        {
            if (_db.Payments.Any(p => p.PaymentId == payment.PaymentId))
                return;

            _db.Payments.Add(payment);
            _db.SaveChanges();
        }

        public PaymentEntity Get(int id)
        {
            return _db.Payments.SingleOrDefault(p => p.Id == id);
        }
    }
}
