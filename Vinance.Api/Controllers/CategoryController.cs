using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;
    using Contracts.Models.Categories;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("income-categories")]
        public async Task<ActionResult> GetIncomeCategories()
        {
            var incomeCategories = await _categoryService.GetAll<IncomeCategory>();
            return Ok(incomeCategories);
        }

        [HttpGet]
        [Route("transfer-categories")]
        public async Task<ActionResult> GetTransferCategories()
        {
            var transferCategories = await _categoryService.GetAll<TransferCategory>();
            return Ok(transferCategories);
        }

        [HttpGet]
        [Route("expense-categories")]
        public async Task<ActionResult> GetExpenseCategories()
        {
            var expenseCategories = await _categoryService.GetAll<ExpenseCategory>();
            return Ok(expenseCategories);
        }
    }
}