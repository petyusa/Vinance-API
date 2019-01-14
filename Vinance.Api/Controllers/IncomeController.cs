using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Logic;
    using Viewmodels.Income;

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

        /// <summary>
        /// Creates a new income.
        /// </summary>
        /// <param name="incomeToCreate">The income to be created.</param>
        [SwaggerResponse(201, Type = typeof(int))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CreateIncomeViewmodel incomeToCreate)
        {
            var income = _mapper.Map<Income>(incomeToCreate);
            await _authorizationService.HandleCreateUpdateAsync(income);
            var createdIncome = await _incomeService.Create(income);
            var model = _mapper.Map<IncomeViewmodel>(createdIncome);
            return Created(Request.Path, model);
        }

        /// <summary>
        /// Gets the incomes of the user in a paginated format.
        /// </summary>
        /// <param name="accountId">If specified, only incomes related to this account will be returned.</param>
        /// <param name="categoryId">If specified, only incomes related to this category will be returned.</param>
        /// <param name="from">If specified, only incomes from this date will be returned.</param>
        /// <param name="to">If specified, only incomes to this date will be returned.</param>
        /// <param name="pageSize">If specified, the given number of incomes will be returned (if not specified, defaults to 20).</param>
        /// <param name="page">If specified, the given page will be returned (if not, defaults to 1).</param>
        /// <param name="order">If specified, the incomes will be sorted by the given order (default date_desc).</param>
        [SwaggerResponse(200, Type = typeof(PagedList<IncomeViewmodel>))]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int? accountId, int? categoryId, DateTime? from, DateTime? to, int page = 1, int pageSize = 20, string order = "date_desc")
        {
            var incomes = await _incomeService.GetAll(accountId, categoryId, from, to, order);
            var model = _mapper.MapAll<IncomeViewmodel>(incomes).ToPagedList(page, pageSize);
            return Ok(model);
        }

        /// <summary>
        /// Gets the income with the specified id.
        /// </summary>
        /// <param name="incomeId">The id of the income to be returned.</param>
        [SwaggerResponse(200, Type = typeof(IncomeViewmodel))]
        [SwaggerResponse(403, Description = "User is not he owner of the income.")]
        [SwaggerResponse(404, Description = "No income found with the given id.")]
        [HttpGet]
        [Route("{incomeId}")]
        public async Task<IActionResult> GetById(int incomeId)
        {
            await _authorizationService.HandleGetDeleteAsync<Income>(incomeId);
            var income = await _incomeService.GetById(incomeId);
            var model = _mapper.Map<IncomeViewmodel>(income);
            return Ok(model);
        }

        /// <summary>
        /// Updates the given income.
        /// </summary>
        /// <param name="incomeToUpdate">The income to be updated.</param>
        [SwaggerResponse(200, Type = typeof(IncomeViewmodel))]
        [SwaggerResponse(403, Description = "User is not he owner of the income.")]
        [SwaggerResponse(404, Description = "No income found with the given id.")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateIncomeViewmodel incomeToUpdate)
        {
            var income = _mapper.Map<Income>(incomeToUpdate);
            await _authorizationService.HandleCreateUpdateAsync(income);
            var updatedIncome = await _incomeService.Update(income);
            var model = _mapper.Map<IncomeViewmodel>(updatedIncome);
            return Ok(model);
        }

        /// <summary>
        /// Deletes the income with the specified id.
        /// </summary>
        /// <param name="incomeId">The id of the income to be deleted.</param>
        [SwaggerResponse(204)]
        [SwaggerResponse(403, Description = "User is not he owner of the income.")]
        [SwaggerResponse(404, Description = "No income found with the given id.")]
        [HttpDelete]
        [Route("{incomeId}")]
        public async Task<IActionResult> Delete(int incomeId)
        {
            await _authorizationService.HandleGetDeleteAsync<Income>(incomeId);
            await _incomeService.Delete(incomeId);
            return NoContent();
        }

        /// <summary>
        /// Uploads multiple incomes from Excel-file.
        /// </summary>
        /// <param name="file">The excel file containing the incomes.</param>
        [SwaggerResponse(200, Type = typeof(List<IncomeViewmodel>))]
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