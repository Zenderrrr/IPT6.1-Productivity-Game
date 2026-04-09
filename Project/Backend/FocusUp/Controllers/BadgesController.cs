using FocusUp.Application.Services;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BadgesController : ControllerBase
    {
        public BadgeService _badgeService;

        public BadgesController(BadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpGet("")]
        public IActionResult GetBadges()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetAvailableBadges();

                return Ok(badges);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("/unlocked")]
        public IActionResult GetUnlockedBadges()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetUnlockedBadges(userId);

                return Ok(badges);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("/locked")]
        public IActionResult GetLockedBadges()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetLockedBadges(userId);

                if (badges == null)
                    return NotFound();

                Ok(badges);
            }catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBadgeById(int id)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var badge = _badgeService.GetBadgeById(id);

                if (badge == null)
                    return NotFound();

                bool hasBadge = _badgeService.HasBadge(userId, badge.Id);
                return Ok( new { badge.Id, badge.Name, badge.Description, badge.RuleType, badge.RuleValue, hasBadge });
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}