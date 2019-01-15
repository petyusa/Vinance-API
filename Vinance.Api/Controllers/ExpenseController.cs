using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vinance.Api.Helpers;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Logic;
    using Viewmodels.Expense;

    [Route("expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseService expenseService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _expenseService = expenseService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new expense.
        /// </summary>
        /// <param name="expenseToCreate">The expense to be created.</param>
        [SwaggerResponse(201, Type = typeof(VinanceApiResponseExample<ExpenseViewmodel>))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CreateExpenseViewmodel expenseToCreate)
        {
            var expense = _mapper.Map<Expense>(expenseToCreate);
            await _authorizationService.HandleCreateUpdateAsync(expense);
            var createdExpense = await _expenseService.Create(expense);
            var model = _mapper.Map<ExpenseViewmodel>(createdExpense);
            return Created(Request.Path, model);
        }

        /// <summary>
        /// Gets the expenses of the user in a paginated format.
        /// </summary>
        /// <param name="accountId">If specified, only expenses related to this account will be returned.</param>
        /// <param name="categoryId">If specified, only expenses related to this category will be returned.</param>
        /// <param name="from">If specified, only expenses from this date will be returned.</param>
        /// <param name="to">If specified, only expenses to this date will be returned.</param>
        /// <param name="pageSize">If specified, the given number of expenses will be returned (if not specified, defaults to 20).</param>
        /// <param name="page">If specified, the given page will be returned (if not, defaults to 1).</param>
        /// <param name="order">If specified, the expenses will be sorted by the given order (default date_desc).</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<PagedList<ExpenseViewmodel>>))]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(int? accountId, int? categoryId, DateTime? from, DateTime? to, int pageSize = 20, int page = 1, string order = null)
        {
            var expenses = await _expenseService.GetAll(accountId, categoryId, from, to, order);
            var list = _mapper.MapAll<ExpenseViewmodel>(expenses).ToPagedList(page, pageSize);
            return Ok(list);
        }

        /// <summary>
        /// Gets the expense with the specified id.
        /// </summary>
        /// <param name="expenseId">The id of the expense to be returned.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<ExpenseViewmodel>))]
        [SwaggerResponse(403, Description = "User is not he owner of the expense.")]
        [SwaggerResponse(404, Description = "No expense found with the given id.")]
        [HttpGet]
        [Route("{expenseId}")]
        public async Task<IActionResult> GetById(int expenseId)
        {
            await _authorizationService.HandleGetDeleteAsync<Expense>(expenseId);
            var expense = await _expenseService.GetById(expenseId);
            var model = _mapper.Map<ExpenseViewmodel>(expense);
            return Ok(model);
        }

        /// <summary>
        /// Updates the given expense.
        /// </summary>
        /// <param name="expenseToUpdate">The expense to be updated.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<ExpenseViewmodel>))]
        [SwaggerResponse(403, Description = "User is not he owner of the expense.")]
        [SwaggerResponse(404, Description = "No expense found with the given id.")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateExpenseViewmodel expenseToUpdate)
        {
            var expense = _mapper.Map<Expense>(expenseToUpdate);
            await _authorizationService.HandleCreateUpdateAsync(expense);
            var updatedExpense = await _expenseService.Update(expense);
            var model = _mapper.Map<ExpenseViewmodel>(updatedExpense);
            return Ok(model);
        }

        /// <summary>
        /// Deletes the expense with the specified id.
        /// </summary>
        /// <param name="expenseId">The id of the expense to be deleted.</param>
        [SwaggerResponse(204)]
        [SwaggerResponse(403, Description = "User is not he owner of the expense.")]
        [SwaggerResponse(404, Description = "No expense found with the given id.")]
        [HttpDelete]
        [Route("{expenseId}")]
        public async Task<IActionResult> Delete(int expenseId)
        {
            await _authorizationService.HandleGetDeleteAsync<Expense>(expenseId);
            await _expenseService.Delete(expenseId);
            return NoContent();
        }

        /// <summary>
        /// Uploads multiple expenses from Excel-file.
        /// </summary>
        /// <param name="file">The excel file containing the expenses.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<List<ExpenseViewmodel>>))]
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var expenses = await _expenseService.Upload(stream);
                var viewmodel = _mapper.MapAll<ExpenseViewmodel>(expenses);
                return Ok(viewmodel);
            }
        }
    }
}