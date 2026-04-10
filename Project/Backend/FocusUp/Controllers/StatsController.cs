using FocusUp.Application.Services;
using FocusUp.Common.Exceptions;
using FocusUp.Domain.Enums;
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
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService;
        public StatsController(StatsService statsService) => _statsService = statsService;

        [HttpGet]
        public IActionResult GetStats([FromQuery] int? rangeInDays, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            if (rangeInDays == null && from == null && to == null)
                rangeInDays = 7;

            try
            {
                var statsDto = _statsService.GetStats(userId, rangeInDays, from, to);
                return Ok(statsDto);
            }
            catch (UserStatsNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("productivity")]
        public IActionResult GetProductivityStats([FromQuery] int? rangeInDays, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            if (rangeInDays == null && from == null && to == null)
                rangeInDays = 7;

            try
            {
                var prodDto = _statsService.GetProductivity(userId, rangeInDays, from, to);
                return Ok(prodDto);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
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