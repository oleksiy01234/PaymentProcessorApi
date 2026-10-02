namespace PaymentProcessorApi.Services
{
    public interface IIdempotencyService
    {
        bool HasBeenProcessed(string eventId);
        bool MarkAsProcessed(string eventId);
    }
}
