using System.Text.Json.Serialization;

namespace PaymentProcessorApi.Models
{
    public record CreatePaymentRequestDto(
        [property: JsonPropertyName("amount")] decimal Amount,
        [property: JsonPropertyName("currency")] string Currency,
        [property: JsonPropertyName("description")] string? Description
    );
}
