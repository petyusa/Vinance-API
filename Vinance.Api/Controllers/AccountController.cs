using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vinance.Api.ActionFilters;
using Vinance.Api.Viewmodels;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;
    using Contracts.Models;

    [Route("api/accounts")]
    [ApiController]
    [ServiceFilter(typeof(HeaderValidationFilterAttribute))]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAll();
            var model = _mapper.Map<IEnumerable<AccountViewmodel>>(accounts);

            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Account account)
        {
            var createdAccount = await _accountService.Create(account);
            if (createdAccount == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the account");
            return Created(Request.Path, createdAccount);
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> Get(int accountId)
        {
            var account = await _accountService.Get(accountId);
            if (account == null)
                return NotFound($"No account found with id: {accountId}");
            return Ok(account);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Account account)
        {
            var updatedAccount = await _accountService.Update(account);
            if (updatedAccount == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro updaeting the account");
            return Created(Request.Path, updatedAccount);

        }

        [HttpDelete]
        [Route("{accountId}")]
        public async Task<IActionResult> Delete(int accountId)
        {
            var success = await _accountService.Delete(accountId);
            if (success)
                return NoContent();
            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro deleting the account");
        }
    }
}