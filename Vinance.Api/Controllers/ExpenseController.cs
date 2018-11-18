using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseService expenseService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _expenseService = expenseService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Expense expense)
        {
            await _authorizationService.HandleCreateUpdateAsync(expense);
            var createdExpense = await _expenseService.Create(expense);
            var model = _mapper.Map<ExpenseViewmodel>(createdExpense);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseService.GetAll();
            var model = _mapper.MapAll<ExpenseViewmodel>(expenses);
            return Ok(model);
        }

        [HttpGet]
        [Route("{expenseId}")]
        public async Task<IActionResult> GetById(int expenseId)
        {
            await _authorizationService.HandleGetDeleteAsync<Expense>(expenseId);
            var expense = await _expenseService.GetById(expenseId);
            var model = _mapper.Map<ExpenseViewmodel>(expense);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Expense expense)
        {
            await _authorizationService.HandleCreateUpdateAsync(expense);
            var updatedExpense = await _expenseService.Update(expense);
            var model = _mapper.Map<ExpenseViewmodel>(updatedExpense);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{expenseId}")]
        public async Task<IActionResult> Delete(int expenseId)
        {
            await _authorizationService.HandleGetDeleteAsync<Expense>(expenseId);
            await _expenseService.Delete(expenseId);
            return NoContent();
        }
    }
}