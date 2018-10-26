using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vinance.Api.Viewmodels;
using Vinance.Contracts.Extensions;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;
    using Contracts.Models.Categories;

    [Route("api")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IExpenseService expenseService, IMapper mapper)
        {
            _categoryService = categoryService;
            _expenseService = expenseService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("income-categories")]
        public async Task<IActionResult> GetIncomeCategories()
        {
            var incomeCategories = await _categoryService.GetAll<IncomeCategory>();
            return Ok(incomeCategories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncomeCategory(IncomeCategory category)
        {
            var createdCategory = await _categoryService.Create(category);
            return Created(Request.Path.Value, createdCategory);
        }

        [HttpGet]
        [Route("transfer-categories")]
        public async Task<IActionResult> GetTransferCategories()
        {
            var transferCategories = await _categoryService.GetAll<TransferCategory>();
            return Ok(transferCategories);
        }

        [HttpGet]
        [Route("expense-categories")]
        public async Task<IActionResult> GetExpenseCategories()
        {
            var expenseCategories = await _categoryService.GetAll<ExpenseCategory>();
            return Ok(expenseCategories);
        }

        [HttpGet]
        [Route("expense-categories/{categoryId}/expenses")]
        public async Task<IActionResult> GetExpensesByCategory(int categoryId)
        {
            var expenses = await _expenseService.GetByCategoryId(categoryId);
            var model = _mapper.MapAll<ExpenseViewmodel>(expenses);
            return Ok(model);
        }
    }
}