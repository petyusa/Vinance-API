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

        //[HttpPost]
        //[Route("accounts")]
        //public async Task<IActionResult> Create([FromBody] Income account)
        //{
        //    var createdAccount = await _incomeService.Create(account);

        //    if (createdAccount == null)
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the account");

        //    return Created(Request.Path, createdAccount);
        //}

        //[HttpGet]
        //[Route("accounts/{accountId}")]
        //public async Task<IActionResult> Get(int accountId)
        //{
        //    var account = await _incomeService.Get(accountId);

        //    if (account == null)
        //        return NotFound(accountId);

        //    return Ok(account);
        //}

        //[HttpPut]
        //[Route("accounts/{accountId}")]
        //public async Task<IActionResult> Update(Income account)
        //{
        //    var updatedAccount = await _incomeService.Update(account);

        //    if (updatedAccount == null)
        //        return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro updaeting the account");

        //    return Created(Request.Path, updatedAccount);

        //}

        //[HttpDelete]
        //[Route("accounts/{accountId}")]
        //public async Task<IActionResult> Delete(int accountId)
        //{
        //    var account = await _incomeService.Get(accountId);

        //    if (account == null)
        //        return NotFound(accountId);

        //    return Ok(account);
        //}
    }
}