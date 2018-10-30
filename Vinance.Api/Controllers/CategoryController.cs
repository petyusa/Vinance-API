using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Enums;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll(CategoryType? type)
        {
            var categories = await _categoryService.GetAll(type);
            return Ok(categories);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Category category)
        {
            var createdCategory = await _categoryService.Create(category);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(createdCategory);
            return Created(Request.Path.Value, categoryViewmodel);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Category category)
        {
            await _categoryService.Update(category);
            var updatedCategory = _categoryService.Get(category.Id);
            var categoryViewmodel = _mapper.Map<CategoryViewmodel>(updatedCategory);
            return Ok(categoryViewmodel);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            await _categoryService.Delete(categoryId);
            return NoContent();
        }
    }
}