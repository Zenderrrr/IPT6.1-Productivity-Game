using FocusUp.Domain.Enums;
using System;
using static FocusUp.Domain.Enums.TaskStatus;
using FocusUp.Common.Exceptions;
using FocusUp.Infrastructure.Repositories;
using FocusUp.Application.DTOs;

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
        
        private readonly DatabaseConnection _dbConnection;

        public TaskCompletionService(TaskRepository taskRepository, XPService xPService, StreakService streakService, LevelService levelService, BadgeService badgeService, UserStatsRepository userStatsRepository, XPEventRepository xPEventRepository, TaskLogRepository taskLogRepository, DatabaseConnection databaseConnection)
        {
            _taskRepository = taskRepository;
            _xPService = xPService;
            _streakService = streakService;
            _levelService = levelService;
            _badgeService = badgeService;
            _userStatsRepository = userStatsRepository;
            _xPEventRepository = xPEventRepository;
            _taskLogRepository = taskLogRepository;
            _dbConnection = databaseConnection;
        }

        public TaskCompleteDto CompleteTask(int taskId, int userId)
        {
            var taskCompleteDto = new TaskCompleteDto();

            DateTime completedAt = DateTime.Now;

            UserStats? userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            
            int streakCountBefore = userStats.StreakCount;

            Task? task = _taskRepository.GetById(taskId) ?? throw new TaskNotFoundException(taskId);
            taskCompleteDto.Task = task;

            if (task.UserId != userId)
                throw new UnauthorizedTaskAccessException(taskId, userId);

            if(task.IsCompleted())
                throw new TaskAlreadyCompletedException(taskId);

            task.MarkAsCompleted();
            int currentStreak = _streakService.CalculateNewStreak(userStats, completedAt);

            taskCompleteDto.StreakCountAfter = currentStreak;
            taskCompleteDto.IsStreakIncreased = streakCountBefore < currentStreak;

            userStats.SetStreak(currentStreak, completedAt);

            int xpAwarded = _xPService.CalculateXP(task, currentStreak);
            userStats.AddXp(xpAwarded);
            taskCompleteDto.Xp = xpAwarded;

            TaskLog taskLog = new(userId, taskId, RewardReason.TaskCompleted, xpAwarded);
            userStats.ApplyTaskCompletion(task.DurationMin, completedAt);

            using var connection = _dbConnection.GetConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                _taskRepository.UpdateStatus(taskId, Completed, connection, transaction, completedAt);

                XpEvent xpEvent = new XpEvent(userId, xpAwarded, RewardReason.TaskCompleted, taskId);
                _xPEventRepository.Insert(xpEvent);

                _taskLogRepository.Insert(taskLog, connection, transaction);
                _userStatsRepository.Update(userStats, connection, transaction);
                taskCompleteDto.Badges = _badgeService.CheckAndAwardBadges(userId, connection, transaction);

                transaction.Commit();
                return taskCompleteDto;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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