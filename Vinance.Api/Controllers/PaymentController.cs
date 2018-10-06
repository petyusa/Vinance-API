using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("payments")]
        public async Task<ActionResult> GetPayments()
        {
            var payments = await _paymentService.GetPayments();

            return Ok(payments);
        }

        [HttpGet]
        [Route("payments/{id}")]
        public async Task<ActionResult> GetPayment(int paymentId)
        {
            var payments = await _paymentService.GetPayments();

            return Ok(payments);
        }
    }
}