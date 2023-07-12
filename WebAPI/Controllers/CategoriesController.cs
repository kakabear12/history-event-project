using AutoMapper;
using BusinessObjectsLayer.Models;
using DTOs.Request;
using DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
        [HttpGet]
       
        [SwaggerOperation(Summary = "For get list of category")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await categoryRepository.GetCategories();
            if(categories.Count() == 0) {
                return NotFound();
            }
            var cates = mapper.Map<List<CategoryResponse>>(categories);
            return Ok(new ResponseObject { 
                Message = "Get list category successfully.",
                Data = cates
            });
        }
        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For create category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cate = mapper.Map<Category>(request);

            await categoryRepository.CreateCategory(cate);
            return Ok(new ResponseObject { 
                Message = "Create category successfully",
                Data = null
            });
        }
        [HttpPut("UpdateCategory")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update category")]
        public async Task<IActionResult> updateCategory([FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cate = mapper.Map<Category>(request);
            await categoryRepository.UpdateCategory(cate);
            return Ok(new ResponseObject
            {
                Message = "Update category successfully",
                Data = request
            });
        }
        [HttpDelete("DeleteCategory")]
        [Authorize(Roles = "Editor")]
        [SwaggerOperation(Summary = "For update category")]
        public async Task<IActionResult> DeleteCategory (int id)
        {
            await categoryRepository.DeleteCategory(id);
            return Ok(new ResponseObject
            {
                Message = "Delete category successfully",
                Data = null
            });
        }
    }
}
