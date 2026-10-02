using System.Text.Json.Serialization;

namespace PaymentProcessorApi.Models
{
    public record WebhookEventDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("created_at")] long CreatedAt,
        [property: JsonPropertyName("data")] PaymentTransactionDto Data
    );
}
