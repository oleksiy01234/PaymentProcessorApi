namespace PaymentProcessorApi.Services
{
    public interface IIdempotencyService
    {
        bool HasBeenProcessed(string eventId);
        bool MarkAsProcessed(string eventId);
        bool TryGetCachedResponse<T>(string key, out T? response);
        void CacheResponse<T>(string key, T response);
    }
}
