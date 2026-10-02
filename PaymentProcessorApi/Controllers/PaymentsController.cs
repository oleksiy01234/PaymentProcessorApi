using Microsoft.AspNetCore.Mvc;
using PaymentProcessorApi.Models;
using PaymentProcessorApi.Services;

namespace PaymentProcessorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentGatewayService paymentGatewayService;
        private readonly IIdempotencyService idempotencyService;

        public PaymentsController(IPaymentGatewayService paymentGatewayService, IIdempotencyService idempotencyService)
        {
            this.paymentGatewayService = paymentGatewayService;
            this.idempotencyService = idempotencyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(string id, CancellationToken cancellationToken)
        {
            var transaction = await paymentGatewayService.GetTransactionAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound(new { message = $"Payment with ID '{id}' was not found." });
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            [FromBody] CreatePaymentRequestDto request,
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
            CancellationToken cancellationToken)
        {
            // 1. Require an Idempotency-Key header for POST operations
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest(new { error = "Missing required 'Idempotency-Key' HTTP header." });
            }

            // 2. Check if we've already processed this key and have a cached response
            if (idempotencyService.TryGetCachedResponse<PaymentTransactionDto>(idempotencyKey, out var cachedResult))
            {
                Response.Headers["X-Cache-Lookup"] = "HIT";
                return Ok(cachedResult);
            }

            // 3. Atomically check and claim processing rights for this key
            if (!idempotencyService.MarkAsProcessed(idempotencyKey))
            {
                // Edge case: another thread is currently processing this key right now
                return Conflict(new { error = "A request with this Idempotency-Key is currently in progress." });
            }

            // 4. Perform the actual business operation (simulating payment creation)
            var amountInCents = (long)(request.Amount * 100);
            var newPayment = new PaymentTransactionDto(
                Id: $"pay_{Guid.NewGuid().ToString("N")[..8]}",
                Amount: amountInCents,
                Currency: request.Currency,
                Status: "succeeded",
                CreatedAt: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            idempotencyService.CacheResponse(idempotencyKey, newPayment);

            Response.Headers["X-Cache-Lookup"] = "MISS";
            return Ok(newPayment);
        }
    }
}
