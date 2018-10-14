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

    [Route("api/incomes")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public IncomeController(IIncomeService incomeService, IMapper mapper)
        {
            _incomeService = incomeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetIncomes()
        {
            var incomes = await _incomeService.GetAll();

            var model = _mapper.MapAll<IncomeViewmodel>(incomes);
            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Income income)
        {
            var createdIncome = await _incomeService.Create(income);
            if (createdIncome == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error creating the income");
            }

            var model = _mapper.Map<IncomeViewmodel>(createdIncome);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("{incomeId}")]
        public async Task<IActionResult> Get(int incomeId)
        {
            var income = await _incomeService.Get(incomeId);
            if (income == null)
            {
                return NotFound($"No income found with id: {incomeId}");
            }

            var model = _mapper.Map<IncomeViewmodel>(income);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Income income)
        {
            var updatedIncome = await _incomeService.Update(income);
            if (updatedIncome == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error updaeting the income");
            }

            var model = _mapper.Map<IncomeViewmodel>(updatedIncome);
            return Ok(model);

        }

        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<IActionResult> Delete(int incomeId)
        {
            var success = await _incomeService.Delete(incomeId);
            if (success)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an error deleting the income");
        }
    }
}