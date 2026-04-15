using FocusUp.Application.DTOs;
using FocusUp.Application.Services;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FocusUp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoriesController(CategoryService categoryService) => _categoryService = categoryService;

        [HttpGet("")]
        public IActionResult GetCategories()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var categories = _categoryService.GetCategoriesByUser(userId).Select(c =>
                        new CategoryDto
                        {
                            Id = c.Id,
                            UserId = c.UserId,
                            Name = c.Name,
                            Color = c.Color,
                            CreatedAt = c.CreatedAt
                        }
                    );

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
            if (!TryGetUserId(out int userId))
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

            if (!TryGetUserId(out int userId))
                return Unauthorized();

            if (_categoryService.CategoryExistsByName(userId, categoryRequest.Name))
                return Conflict();

            try
            {
                var category = new Category(userId, categoryRequest.Name, categoryRequest.Color);

                if(!category.ValidateData())
                    return BadRequest();

                int categoryId = _categoryService.CreateCategory(category);

                return StatusCode(201, categoryId);
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

            if (!TryGetUserId(out int userId))
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
            if (!TryGetUserId(out int userId))
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

        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            var userIdClaim =
                User.FindFirst("sub")?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return false;

            return int.TryParse(userIdClaim, out userId);
        }
    }
}