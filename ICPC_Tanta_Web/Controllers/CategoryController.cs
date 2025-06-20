using Core.DTO;
using Core.Entities;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Instructor")]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _unitOfWork.ContentCategoryRepository.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<ContentCategory>>.SuccessResponse("Categories retrieved successfully.", categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.ContentCategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Category not found."));

                return Ok(ApiResponse<ContentCategory>.SuccessResponse("Category retrieved successfully.", category));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid model."));

            try
            {
                var category = new ContentCategory
                {
                    CategoryName = dto.CategoryName
                };

                await _unitOfWork.ContentCategoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Category created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDto dto)
        {
            try
            {
                var category = await _unitOfWork.ContentCategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Category not found."));

                category.CategoryName = dto.CategoryName;

                _unitOfWork.ContentCategoryRepository.Update(category);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Category updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.ContentCategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Category not found."));

                _unitOfWork.ContentCategoryRepository.Delete(category);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Category deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
