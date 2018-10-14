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

    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseService expenseService, IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Expense expense)
        {
            var createdExpense = await _expenseService.Create(expense);
            if (createdExpense == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error creating the expense");
            }

            var model = _mapper.Map<ExpenseViewmodel>(createdExpense);
            return Ok(model);
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
            var expense = await _expenseService.GetById(expenseId);
            if (expense == null)
            {
                return NotFound($"No account found with id: {expenseId}");
            }

            var model = _mapper.Map<ExpenseViewmodel>(expense);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] Expense expense)
        {
            var updatedExpense = await _expenseService.Update(expense);
            if (updatedExpense == null)
            {
                return NotFound("There was an error updating the expense");
            }

            var model = _mapper.Map<ExpenseViewmodel>(updatedExpense);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{expenseId}")]
        public async Task<IActionResult> Delete(int expenseId)
        {
            var success = await _expenseService.Delete(expenseId);
            if (success)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error deleting the expense");
        }
    }
}