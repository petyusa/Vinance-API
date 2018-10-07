using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vinance.Contracts.Models;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Payment payment)
        {
            var createdPayment = await _paymentService.Create(payment);
            if(createdPayment == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the payment");
            return Ok(createdPayment);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAll();
            return Ok(payments);
        }

        [HttpGet]
        [Route("{paymentId}")]
        public async Task<IActionResult> GetById(int paymentId)
        {
            var payment = await _paymentService.GetById(paymentId);
            if (payment == null)
                return NotFound($"No account found with id: {paymentId}");
            return Ok(payment);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] Payment payment)
        {
            var updatedPayment = await _paymentService.Update(payment);
            if (updatedPayment == null)
                return NotFound("There was an error updating the payment");
            return Ok(updatedPayment);
        }

        [HttpDelete]
        [Route("{paymentId}")]
        public async Task<IActionResult> Delete(int paymentId)
        {
            var success = await _paymentService.Delete(paymentId);
            if (success)
                return NoContent();
            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro deleting the payment");
        }
    }
}