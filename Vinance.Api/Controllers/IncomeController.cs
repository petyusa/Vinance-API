using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Logic;
    using Viewmodels;

    [Route("incomes")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public IncomeController(IIncomeService incomeService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _incomeService = incomeService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Income income)
        {
            await _authorizationService.HandleCreateUpdateAsync(income);
            var createdIncome = await _incomeService.Create(income);
            var model = _mapper.Map<IncomeViewmodel>(createdIncome);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int? categoryId, DateTime? from, DateTime? to, int page = 1, int pageSize = 20, string order = "date_desc")
        {
            var incomes = await _incomeService.GetAll(categoryId, from, to, order);
            var model = _mapper.MapAll<IncomeViewmodel>(incomes).ToPagedList(page, pageSize);
            return Ok(model);
        }

        [HttpGet]
        [Route("{incomeId}")]
        public async Task<IActionResult> GetById(int incomeId)
        {
            await _authorizationService.HandleGetDeleteAsync<Income>(incomeId);
            var income = await _incomeService.GetById(incomeId);
            var model = _mapper.Map<IncomeViewmodel>(income);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Income income)
        {
            await _authorizationService.HandleCreateUpdateAsync(income);
            var updatedIncome = await _incomeService.Update(income);
            var model = _mapper.Map<IncomeViewmodel>(updatedIncome);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<IActionResult> Delete(int incomeId)
        {
            await _authorizationService.HandleGetDeleteAsync<Income>(incomeId);
            await _incomeService.Delete(incomeId);
            return NoContent();
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var incomes = await _incomeService.Upload(stream);
                var viewmodel = _mapper.MapAll<IncomeViewmodel>(incomes);
                return Ok(viewmodel);
            }
        }
    }
}