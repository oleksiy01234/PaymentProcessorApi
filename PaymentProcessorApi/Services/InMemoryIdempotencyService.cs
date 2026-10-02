using System.Collections.Concurrent;

namespace PaymentProcessorApi.Services
{
    public class InMemoryIdempotencyService : IIdempotencyService
    {
        private readonly ConcurrentDictionary<string, byte> processedEventIds = new();
        private readonly ConcurrentDictionary<string, object> cachedResponses = new();

        public bool HasBeenProcessed(string eventId)
        {
            return processedEventIds.ContainsKey(eventId);
        }

        public bool MarkAsProcessed(string eventId)
        {
            return processedEventIds.TryAdd(eventId, 0);
        }

        public bool TryGetCachedResponse<T>(string key, out T? response)
        {
            if (cachedResponses.TryGetValue(key, out var cachedObj) && cachedObj is T typedResponse)
            {
                response = typedResponse;
                return true;
            }

            response = default;
            return false;
        }

        public void CacheResponse<T>(string key, T response)
        {
            if (response != null)
            {
                cachedResponses[key] = response;
            }
        }
    }
}
