using FocusUp.Domain.Enums;
using System;
using static FocusUp.Domain.Enums.TaskStatus;
using FocusUp.Common.Exceptions;

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
        private readonly TaskLogRepository _taskLogRepository;

        public TaskCompletionService(TaskRepository taskRepository, XPService xPService, StreakService streakService, LevelService levelService, BadgeService badgeService, UserStatsRepository userStatsRepository, XPEventRepository xPEventRepository, TaskLogRepository taskLogRepository)
        {
            _taskRepository = taskRepository;
            _xPService = xPService;
            _streakService = streakService;
            _levelService = levelService;
            _badgeService = badgeService;
            _userStatsRepository = userStatsRepository;
            _xPEventRepository = xPEventRepository;
            _taskLogRepository = taskLogRepository;
        }

        public void CompleteTask(int taskId, int userId)
        {
            DateTime completedAt = DateTime.Now;

            UserStats? userStats = _userStatsRepository.GetById(userId) ?? throw new UserStatsNotFoundException(userId);

            Task? task = _taskRepository.GetById(taskId) ?? throw new TaskNotFoundException(taskId);

            if (task.UserId != userId)
                throw new UnauthorizedTaskAccessException(taskId, userId);

            if(task.IsCompleted())
                throw new TaskAlreadyCompletedException(taskId);

            task.MarkAsCompleted();
            _taskRepository.UpdateStatus(taskId, Done, completedAt);

            _streakService.UpdateStreak(userId, completedAt);
            int currentStreak = _streakService.GetCurrentStreak(userId);

            int xpAwarded = _xPService.CalculateXP(task, _streakService.GetCurrentStreak(userId));

            TaskLog taskLog = new TaskLog(userId, taskId, RewardReason.TaskCompleted, xpAwarded);
            _taskLogRepository.Insert(taskLog);

            _xPService.AwardXP(userId, task, currentStreak, RewardReason.TaskCompleted);

            userStats.ApplyTaskCompletion(task.DurationMin, completedAt);
            _userStatsRepository.Update(userStats);

            int currentLevel = _levelService.GetLevel(userStats.TotalXp);
            double progressNextLevel = _levelService.GetProgressToNextLevel(userStats.TotalXp);

            _badgeService.CheckAndAwardBadges(userId);
        }

        public bool CanCompleteTask(int taskId, int userId)
        {
            Task? task = _taskRepository.GetById(taskId);

            if(task == null)
                return false;

            if(task.UserId != userId) return false;

            if(task.IsCompleted()) return false;

            if(!task.ValidateData()) return false;

            bool isAlreadyCompletedTaskLog = _taskLogRepository.GetAllByTaskId(taskId).Any(t => t.Action == RewardReason.TaskCompleted);
            if(isAlreadyCompletedTaskLog) return false;

            bool isAlreadyCompletedXpEvent = _xPEventRepository.GetAllByTaskId(taskId).Any(t => t.Reason == RewardReason.TaskCompleted);
            if (isAlreadyCompletedXpEvent) return false;

            return true;
        }

        public bool IsAlreadyCompleted(int taskId)
        {
            Task? task = _taskRepository.GetById(taskId);
            if(task == null) return false;
            return task.IsCompleted();
        }
    }
}