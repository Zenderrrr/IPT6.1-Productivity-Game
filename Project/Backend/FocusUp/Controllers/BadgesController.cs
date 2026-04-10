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
    public class BadgesController : ControllerBase
    {
        private readonly BadgeService _badgeService;

        public BadgesController(BadgeService badgeService) => _badgeService = badgeService;

        [HttpGet]
        public IActionResult GetBadges()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetAvailableBadges().Select(b =>
                    new BadgesDto{
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        RuleType = b.RuleType,
                        RuleValue = b.RuleValue,
                        CreatedAt = b.CreatedAt,
                    }
                );

                return Ok(badges);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("unlocked")]
        public IActionResult GetUnlockedBadges()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetUnlockedBadges(userId).Select(b =>
                    new BadgesDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        RuleType = b.RuleType,
                        RuleValue = b.RuleValue,
                        CreatedAt = b.CreatedAt,
                    }
                );

                return Ok(badges);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("locked")]
        public IActionResult GetLockedBadges()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var badges = _badgeService.GetLockedBadges(userId).Select(b =>
                    new BadgesDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Description = b.Description,
                        RuleType = b.RuleType,
                        RuleValue = b.RuleValue,
                        CreatedAt = b.CreatedAt,
                    }
                );

                return Ok(badges);
            }catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBadgeById(int id)
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var badge = _badgeService.GetBadgeById(id);

                if (badge == null)
                    return NotFound();

                bool hasBadge = _badgeService.HasBadge(userId, badge.Id);
                return Ok( new { badge.Id, badge.Name, badge.Description, badge.RuleType, badge.RuleValue, badge.CreatedAt, hasBadge });
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