using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public PaymentController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Route("payments")]
        public async Task<ActionResult> GetPayments()
        {
            var payments = await _transactionService.GetPayments();

            return Ok(payments);
        }
    }
}