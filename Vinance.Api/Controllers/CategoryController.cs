using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Enums;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

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

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(CategoryType? type, DateTime? from, DateTime? to)
        {
            var categories = await _categoryService.GetAll(type, from, to);
            var categoryViewmodels = _mapper.MapAll<CategoryViewmodel>(categories);
            return Ok(categoryViewmodels);
        }

        [HttpGet]
        [Route("{categoryId:int}")]
        public async Task<IActionResult> Get(int categoryId)
        {
            var categories = await _categoryService.Get(categoryId);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(categories);
            return Ok(categoryViewmodel);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CategoryViewmodel categoryViewmodel)
        {
            var category = _mapper.Map<Category>(categoryViewmodel);
            var createdCategory = await _categoryService.Create(category);
            categoryViewmodel = _mapper.Map<CategoryViewmodel>(createdCategory);
            return Created(Request.Path.Value, categoryViewmodel);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(CategoryViewmodel categoryViewmodel)
        {
            var category = _mapper.Map<Category>(categoryViewmodel);
            await _authorizationService.HandleCreateUpdateAsync(category);
            await _categoryService.Update(category);
            var updatedCategory = await _categoryService.Get(category.Id);
            categoryViewmodel = _mapper.Map<CategoryViewmodel>(updatedCategory);
            return Ok(categoryViewmodel);
        }

        [HttpDelete]
        [Route("{categoryId:int}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            await _authorizationService.HandleGetDeleteAsync<Category>(categoryId);
            await _categoryService.Delete(categoryId);
            return NoContent();
        }

        [HttpGet]
        [Route("average")]
        public async Task<IActionResult> GetAverage(CategoryType? type, DateTime? from = null, DateTime? to = null, string by = null)
        {
            var result = await _categoryService.GetStats(type, from, to, by);
            return Ok(result);
        }
    }
}