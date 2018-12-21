using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _accountService = accountService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAll();
            var model = _mapper.MapAll<AccountViewmodel>(accounts);

            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Account account)
        {
            var createdAccount = await _accountService.Create(account);
            var model = _mapper.Map<AccountViewmodel>(createdAccount);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            await _authorizationService.HandleGetDeleteAsync<Account>(accountId);
            var account = await _accountService.Get(accountId);
            if (account == null)
            {
                return NotFound($"No account found with id: {accountId}");
            }
            var model = _mapper.Map<AccountViewmodel>(account);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Account account)
        {
            await _authorizationService.HandleCreateUpdateAsync(account);
            var updatedAccount = await _accountService.Update(account);

            var model = _mapper.Map<AccountViewmodel>(updatedAccount);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{accountId}")]
        public async Task<IActionResult> Delete(int accountId)
        {
            await _authorizationService.HandleGetDeleteAsync<Account>(accountId);
            await _accountService.Delete(accountId);
            return NoContent();
        }

        [HttpGet]
        [Route("daily-balances")]
        public async Task<IActionResult> GetDailyBalances(int? accountId = null, DateTime? from = null, DateTime? to = null)
        {
            var balances = _accountService.GetDailyBalances(accountId, from, to);
            return Ok(balances);
        }
    }
}