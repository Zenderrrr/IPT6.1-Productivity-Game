using System;

namespace FocusUp.Application.Services
{
    public class TaskCompletionService
    {
        private readonly TaskRepository _taskRepository;
        private readonly XPService _xPService;
        private readonly StreakService _streakService;
        private readonly LevelService _levelService;
        private readonly BadgeService _badgeService;
        private readonly UserStatsRepository _userStatsRepository;
        private readonly XPEventRepository _xPEventRepository;

        public TaskCompletionService(TaskRepository taskRepository, XPService xPService, StreakService streakService, LevelService levelService, BadgeService badgeService, UserStatsRepository userStatsRepository, XPEventRepository xPEventRepository)
        {
            _taskRepository = taskRepository;
            _xPService = xPService;
            _streakService = streakService;
            _levelService = levelService;
            _badgeService = badgeService;
            _userStatsRepository = userStatsRepository;
            _xPEventRepository = xPEventRepository;
        }

        public void CompleteTask(int taskId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool CanCompleteTask(int taskId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool IsAlreadyCompleted(int taskId)
        {
            throw new NotImplementedException();
        }
    }
}