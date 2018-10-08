using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vinance.Contracts.Models;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/incomes")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetIncomes()
        {
            var incomes = await _incomeService.GetAll();

            return Ok(incomes);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Income income)
        {
            var createdIncome = await _incomeService.Create(income);
            if (createdIncome == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the income");
            return Created(Request.Path, createdIncome);
        }

        [HttpGet]
        [Route("{incomeId}")]
        public async Task<IActionResult> Get(int incomeId)
        {
            var income = await _incomeService.Get(incomeId);
            if (income == null)
                return NotFound($"No income found with id: {incomeId}");
            return Ok(income);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Income income)
        {
            var updatedIncome = await _incomeService.Update(income);
            if (updatedIncome == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro updaeting the income");
            return Created(Request.Path, updatedIncome);

        }

        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<IActionResult> Delete(int incomeId)
        {
            var success = await _incomeService.Delete(incomeId);
            if (success)
                return NoContent();
            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro deleting the income");
        }
    }
}