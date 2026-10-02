using Microsoft.AspNetCore.Mvc;
using PaymentProcessorApi.Models;

namespace PaymentProcessorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockGatewayController : ControllerBase
    {
        [HttpGet("payments/{id}")]
        public IActionResult GetPayment(string id)
        {
            var transaction = new PaymentTransactionDto(
                Id: id,
                Amount: 5000,
                Currency: "usd",
                Status: "succeeded",
                CreatedAt: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            return Ok(transaction);
        }
    }
}
