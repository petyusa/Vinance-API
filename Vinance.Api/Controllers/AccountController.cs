using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        [Route("accounts")]
        public async Task<ActionResult> GetAccounts()
        {
            var accounts = await _accountService.GetAll();

            return Ok(accounts);
        }
    }
}