using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public CategoryController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
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
        //[Route("expense-categories")]
        //public async Task<ActionResult> GetExpenseCategories()
        //{
        //    var expenseCategories = await _transactionService.GetCategory<ExpenseCategory>();

        //    return Ok(expenseCategories);
        //}
    }
}