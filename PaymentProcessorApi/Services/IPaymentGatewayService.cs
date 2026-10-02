using PaymentProcessorApi.Models;

namespace PaymentProcessorApi.Services
{
    public interface IPaymentGatewayService
    {
        Task<PaymentTransactionDto?> GetTransactionAsync(string id, CancellationToken cancellationToken = default);
    }
}
