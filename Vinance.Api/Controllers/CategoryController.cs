using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public CategoryController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //[HttpGet]
        //[Route("income-categories")]
        //public async Task<ActionResult> GetIncomeCategories()
        //{
        //    var incomeCategories = await _transactionService.GetCategory<IncomeCategory>();

        //    return Ok(incomeCategories);
        //}

        //[HttpGet]
        //[Route("transfer-categories")]
        //public async Task<ActionResult> GetTransferCategories()
        //{
        //    var transferCategories = await _transactionService.GetCategory<TransferCategory>();

        //    return Ok(transferCategories);
        //}

        //[HttpGet]
        //[Route("payment-categories")]
        //public async Task<ActionResult> GetPaymentCategories()
        //{
        //    var paymentCategories = await _transactionService.GetCategory<PaymentCategory>();

        //    return Ok(paymentCategories);
        //}
    }
}