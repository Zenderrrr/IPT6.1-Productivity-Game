using FocusUp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FocusUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly TaskLogService _taskLogService;
        private readonly XpEventService _xpEventService;
        private readonly UserStatsService _userStatsService;
        public StatsController()
        {
            
        }
    }
}