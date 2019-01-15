using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vinance.Api.Helpers;

namespace Vinance.Api.Controllers
{
    using Contracts.Enums;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels.Account;

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

        /// <summary>
        /// Gets the accounts of the user.
        /// </summary>
        /// <param name="accountType">If specified, returns only accounts of the given type.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<List<AccountViewmodel>>))]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(AccountType? accountType = null)
        {
            var accounts = await _accountService.GetAll(accountType);
            var model = _mapper.MapAll<AccountViewmodel>(accounts);

            return Ok(model);
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="createAccount">The account to be created.</param>
        [SwaggerResponse(201, Type = typeof(VinanceApiResponseExample<AccountViewmodel>))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CreateAccountViewmodel createAccount)
        {
            var account = _mapper.Map<Account>(createAccount);
            var createdAccount = await _accountService.Create(account);
            var model = _mapper.Map<AccountViewmodel>(createdAccount);
            return Created(Request.Path, model);
        }

        /// <summary>
        /// Gets the account with the specified id.
        /// </summary>
        /// <param name="accountId">The id of the account to get.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<AccountViewmodel>))]
        [SwaggerResponse(404, Description = "Account not found with the specified id.")]
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

        /// <summary>
        /// Updates the given account.
        /// </summary>
        /// <param name="accountToUpdate">The updated account.</param>
        [SwaggerResponse(404, Description = "Account not found with the specified id.")]
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<AccountViewmodel>))]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateAccountViewmodel accountToUpdate)
        {
            var account = _mapper.Map<Account>(accountToUpdate);
            await _authorizationService.HandleCreateUpdateAsync(account);
            var updatedAccount = await _accountService.Update(account);

            var model = _mapper.Map<AccountViewmodel>(updatedAccount);
            return Ok(model);
        }

        /// <summary>
        /// Deletes the given account.
        /// </summary>
        /// <param name="accountId">The id of the account to be deleted.</param>
        [SwaggerResponse(404, Description = "Account not found with the specified id.")]
        [SwaggerResponse(204)]
        [HttpDelete]
        [Route("{accountId}")]
        public async Task<IActionResult> Delete(int accountId)
        {
            await _authorizationService.HandleGetDeleteAsync<Account>(accountId);
            await _accountService.Delete(accountId);
            return NoContent();
        }

        /// <summary>
        /// Gets the daily balance of active accounts for all days between the specified dates.
        /// </summary>
        /// <param name="accountId">The id of the account (if not specified, all of the accounts daily balances are returned).</param>
        /// <param name="accountType">The type of the accounts (if not specified, all of the accounts' daily balances are returned).</param>
        /// <param name="from">The starting date (if not specified, the last 30 days' daily balances are returned).</param>
        /// <param name="to">The ending date (if not specified, the last 30 days' daily balances are returned).</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<List<DailyBalanceList>>))]
        [HttpGet]
        [Route("daily-balances")]
        public async Task<IActionResult> GetDailyBalances(int? accountId = null, AccountType? accountType = null, DateTime? from = null, DateTime? to = null)
        {
            var balances = _accountService.GetDailyBalances(accountId, accountType, from, to);
            var viewmodel = _mapper.MapAll<DailyBalanceList>(balances);
            return Ok(viewmodel);
        }
    }
}