using FocusUp.Application.DTOs;
using FocusUp.Application.Services;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoriesController(CategoryService categoryService) => _categoryService = categoryService;

        [HttpGet("")]
        public IActionResult GetCategories()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var categories = _categoryService.GetCategoriesByUser(userId);

                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var category = _categoryService.GetCategoryById(id);

                if(category == null)
                    return NotFound();

                if (category.UserId != userId)
                    return Forbid();

                return Ok(new { category.Name, category.Color });
            } catch(Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPost("")]
        public IActionResult CreateCategory([FromBody] CategoryRequest categoryRequest)
        {
            if(categoryRequest == null)
                return BadRequest();

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            if (_categoryService.CategoryExistsByName(userId, categoryRequest.Name))
                return Conflict();

            try
            {
                var category = new Category(userId, categoryRequest.Name, categoryRequest.Color);

                if(!category.ValidateData())
                    return BadRequest();

                int categoryId = _categoryService.CreateCategory(category);

                return StatusCode(201, new { categoryId });
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
                return BadRequest();

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var category = _categoryService.GetCategoryById(id);

                if(category == null)
                    return NotFound();

                if (category.UserId != userId)
                    return Forbid();

                category.Rename(categoryRequest.Name);
                category.ChangeColor(categoryRequest.Color);

                if (!category.ValidateData())
                    return BadRequest();

                _categoryService.UpdateCategory(category);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var category = _categoryService.GetCategoryById(id);
                if (category == null)
                    return NotFound();

                if (category.UserId != userId)
                    return Forbid();

                _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}