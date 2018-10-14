using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Payment payment)
        {
            var createdPayment = await _paymentService.Create(payment);
            if (createdPayment == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error creating the payment");
            }

            var model = _mapper.Map<PaymentViewmodel>(createdPayment);
            return Ok(model);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAll();
            var model = _mapper.MapAll<PaymentViewmodel>(payments);
            return Ok(model);
        }

        [HttpGet]
        [Route("{paymentId}")]
        public async Task<IActionResult> GetById(int paymentId)
        {
            var payment = await _paymentService.GetById(paymentId);
            if (payment == null)
            {
                return NotFound($"No account found with id: {paymentId}");
            }

            var model = _mapper.Map<PaymentViewmodel>(payment);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] Payment payment)
        {
            var updatedPayment = await _paymentService.Update(payment);
            if (updatedPayment == null)
            {
                return NotFound("There was an error updating the payment");
            }

            var model = _mapper.Map<PaymentViewmodel>(updatedPayment);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{paymentId}")]
        public async Task<IActionResult> Delete(int paymentId)
        {
            var success = await _paymentService.Delete(paymentId);
            if (success)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error deleting the payment");
        }
    }
}