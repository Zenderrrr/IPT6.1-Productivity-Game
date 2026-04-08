using FocusUp.Application.Services;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FocusUp.Controllers
{
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

        [HttpGet("")]
        public IActionResult GetDashboardData()
        {
            int taskLogLimit = 20;

            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var userStats = _userStatsService.GetUserStats(userId);

                if(userStats == null)
                    return NotFound();

                var recentTasks = _taskLogService.GetRecentLogs(userId, taskLogLimit);

                int currLevel = _levelService.GetLevel(userStats.TotalXp);
                int xpNextLevel = _levelService.GetXpForNextLevel(userStats.TotalXp);
                double progressNextLevel = _levelService.GetProgressToNextLevel(userStats.TotalXp);

                return Ok(new { userStats.TotalXp, userStats.TasksDone, userStats.TasksOpen, userStats.StreakCount, userStats.BestStreak, userStats.TotalTimeMin, recentTasks, currLevel, xpNextLevel, progressNextLevel});
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}