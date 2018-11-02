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

    [Route("api/accounts")]
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
            if (createdAccount == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error creating the account");
            }

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
            if (updatedAccount == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error updating the account");
            }

            var model = _mapper.Map<AccountViewmodel>(account);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{accountId}")]
        public async Task<IActionResult> Delete(int accountId)
        {
            await _authorizationService.HandleGetDeleteAsync<Account>(accountId);
            var success = await _accountService.Delete(accountId);
            if (success)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error deleting the account");
        }
    }
}