using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("incomes")]
        public async Task<ActionResult> GetIncomes()
        {
            var incomes = await _incomeService.GetAll();

            return Ok(incomes);
        }
    }
}