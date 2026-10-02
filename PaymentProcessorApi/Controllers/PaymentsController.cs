using Microsoft.AspNetCore.Mvc;
using PaymentProcessorApi.Services;

namespace PaymentProcessorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentGatewayService paymentGatewayService;

        public PaymentsController(IPaymentGatewayService paymentGatewayService)
        {
            this.paymentGatewayService = paymentGatewayService;
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
    }
}
