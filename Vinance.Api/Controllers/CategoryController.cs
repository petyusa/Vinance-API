using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vinance.Api.Controllers
{
    using Contracts.Enums;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Contracts.Models.Helpers;
    using Viewmodels.Category;

    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _categoryService = categoryService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all the categories of the user.
        /// </summary>
        /// <param name="type">If specified, returns only categories of the given type.</param>
        /// <param name="from">The start date for calculating category-balance (if not specified, balance is calculated for all time).</param>
        /// <param name="to">The end date for calculating category-balance (if not specified, balance is calculated for all time).</param>
        [SwaggerResponse(200, Type = typeof(List<CategoryViewmodel>))]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(CategoryType? type, DateTime? from, DateTime? to)
        {
            var categories = await _categoryService.GetAll(type, from, to);
            var categoryViewmodels = _mapper.MapAll<CategoryViewmodel>(categories);
            return Ok(categoryViewmodels);
        }

        /// <summary>
        /// Gets the category with the specified id.
        /// </summary>
        /// <param name="categoryId">The id of the category to be returned.</param>
        [SwaggerResponse(200, Type = typeof(CategoryViewmodel))]
        [SwaggerResponse(404, Description = "Category not found with the specified id.")]
        [HttpGet]
        [Route("{categoryId:int}")]
        public async Task<IActionResult> Get(int categoryId)
        {
            var categories = await _categoryService.Get(categoryId);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(categories);
            return Ok(categoryViewmodel);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryToCreate">The category to be created.</param>
        [SwaggerResponse(201, Type = typeof(CategoryViewmodel))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CreateCategoryViewmodel categoryToCreate)
        {
            var category = _mapper.Map<Category>(categoryToCreate);
            var createdCategory = await _categoryService.Create(category);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(createdCategory);
            return Created(Request.Path.Value, categoryViewmodel);
        }

        /// <summary>
        /// Updates the given category.
        /// </summary>
        /// <param name="categoryToUpdate">The category to be updated.</param>
        [SwaggerResponse(200, Type = typeof(CategoryViewmodel))]
        [SwaggerResponse(404, Description = "Category not found with the specified id.")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateCategoryViewmodel categoryToUpdate)
        {
            var category = _mapper.Map<Category>(categoryToUpdate);
            await _authorizationService.HandleCreateUpdateAsync(category);
            await _categoryService.Update(category);
            var updatedCategory = await _categoryService.Get(category.Id);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(updatedCategory);
            return Ok(categoryViewmodel);
        }

        /// <summary>
        /// Deletes the category with the specified id.
        /// </summary>
        /// <param name="categoryId">The id of the category to be deleted.</param>
        [SwaggerResponse(204)]
        [SwaggerResponse(404, Description = "Category not found with the specified id.")]
        [HttpDelete]
        [Route("{categoryId:int}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            await _authorizationService.HandleGetDeleteAsync<Category>(categoryId);
            await _categoryService.Delete(categoryId);
            return NoContent();
        }

        /// <summary>
        /// Gets the balance for each category grouped by month.
        /// </summary>
        /// <param name="type">The type of the categories to be returned (if not specified, expense-categories are returned).</param>
        /// <param name="from">The start date of the query (if not specified, all category-balances are returned).</param>
        /// <param name="to">The end date of the query (if not specified, all category-balances are returned).</param>
        [SwaggerResponse(200, Type = typeof(List<CategoryStatistics>))]
        [HttpGet]
        [Route("stats")]
        public async Task<IActionResult> GetStats(CategoryType? type, DateTime? from = null, DateTime? to = null)
        {
            var result = await _categoryService.GetStats(type, from, to);
            return Ok(result);
        }
    }
}