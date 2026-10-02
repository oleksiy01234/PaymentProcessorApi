using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessorApi.Models;
using PaymentProcessorApi.Services;

namespace PaymentProcessorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly IIdempotencyService idempotencyService;

        public WebhooksController(IIdempotencyService idempotencyService)
        {
            this.idempotencyService = idempotencyService;
        }

        [HttpPost]
        public IActionResult ReceiveWebhook([FromBody] WebhookEventDto webhook)
        {
            if (!idempotencyService.MarkAsProcessed(webhook.Id))
            {
                return Ok(new
                {
                    status = "ignored",
                    message = $"Event '{webhook.Id}' was already processed."
                });
            }

            return Ok(new
            {
                status = "processed",
                eventId = webhook.Id,
                paymentId = webhook.Data.Id
            });
        }
    }
}
