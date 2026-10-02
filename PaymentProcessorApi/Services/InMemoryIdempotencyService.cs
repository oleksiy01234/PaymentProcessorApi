using System.Collections.Concurrent;

namespace PaymentProcessorApi.Services
{
    public class InMemoryIdempotencyService : IIdempotencyService
    {
        private readonly ConcurrentDictionary<string, byte> processedEventIds = new();

        public bool HasBeenProcessed(string eventId)
        {
            return processedEventIds.ContainsKey(eventId);
        }

        public bool MarkAsProcessed(string eventId)
        {
            return processedEventIds.TryAdd(eventId, 0);
        }
    }
}
