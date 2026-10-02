using System.Text.Json.Serialization;

namespace PaymentProcessorApi.Models
{
    public record PaymentTransactionDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("amount")] long Amount,
        [property: JsonPropertyName("currency")] string Currency,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("created_at")] long CreatedAt
    );
}
