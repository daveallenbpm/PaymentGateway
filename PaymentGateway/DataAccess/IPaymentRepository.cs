namespace PaymentGateway.DataAccess
{
    public interface IPaymentRepository
    {
        void Add(PaymentEntity payment);
        PaymentEntity Get(int id);
        PaymentEntity GetByPaymentId(System.Guid paymentId);
    }
}