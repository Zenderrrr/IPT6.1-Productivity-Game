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
    public class DashboardController : ControllerBase
    {
        private readonly UserStatsService _userStatsService;
        private readonly TaskLogService _taskLogService;
        private readonly LevelService _levelService;

        public DashboardController(UserStatsService userStatsService, TaskLogService taskLogService, LevelService levelService)
        {
            _userStatsService = userStatsService;
            _taskLogService = taskLogService;
            _levelService = levelService;
        }

        [HttpGet]
        public IActionResult GetDashboardData([FromQuery] int? taskLogLimit)
        {
            int limit = taskLogLimit ?? 20;

            if(!TryGetUserId(out int userId))
                return Unauthorized();

            try
            {
                var userStats = _userStatsService.GetUserStats(userId);

                if(userStats == null)
                    return NotFound();

                var lastCompletedTasks = _taskLogService.GetRecentLogs(userId, limit);

                int level = _levelService.GetLevel(userStats.TotalXp);
                int xpNext = _levelService.GetXpForNextLevel(userStats.TotalXp);
                double progressToNextLevel = _levelService.GetProgressToNextLevel(userStats.TotalXp);

                int xpCurrent = _levelService.GetXpCurrentLevel(userStats.TotalXp);

                return Ok(new { userStats.TotalXp, userStats.TasksDone, userStats.TasksOpen, userStats.StreakCount, userStats.BestStreak, userStats.TotalTimeMin, lastCompletedTasks, level, xpNext, progressToNextLevel, xpCurrent });
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