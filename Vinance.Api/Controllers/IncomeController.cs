using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("api/incomes")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public IncomeController(IIncomeService incomeService, IMapper mapper, IAccountService accountService)
        {
            _incomeService = incomeService;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Income income)
        {
            await _accountService.CheckOwner(income.ToId);
            var createdIncome = await _incomeService.Create(income);
            var model = _mapper.Map<IncomeViewmodel>(createdIncome);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var incomes = await _incomeService.GetAll();
            var model = _mapper.MapAll<IncomeViewmodel>(incomes);
            return Ok(model);
        }

        [HttpGet]
        [Route("{incomeId}")]
        public async Task<IActionResult> GetById(int incomeId)
        {
            var income = await _incomeService.GetById(incomeId);
            var model = _mapper.Map<IncomeViewmodel>(income);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Income income)
        {
            await _accountService.CheckOwner(income.ToId);
            var updatedIncome = await _incomeService.Update(income);
            var model = _mapper.Map<IncomeViewmodel>(updatedIncome);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<IActionResult> Delete(int incomeId)
        {
            await _incomeService.Delete(incomeId);
            return NoContent();
        }
    }
}