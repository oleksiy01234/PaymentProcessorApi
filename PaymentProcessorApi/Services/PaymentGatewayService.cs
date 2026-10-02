using PaymentProcessorApi.Models;
using System.Net.Http.Json;

namespace PaymentProcessorApi.Services
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient httpClient;

        public PaymentGatewayService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PaymentTransactionDto?> GetTransactionAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"api/mockgateway/payments/{id}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PaymentTransactionDto>(cancellationToken: cancellationToken);
        }
    }
}