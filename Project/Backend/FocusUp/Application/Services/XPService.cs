using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using System;
using FocusUp.Common.Exceptions;
using Microsoft.Data.Sqlite;

namespace FocusUp.Application.Services
{
    public class XPService
    {
        private readonly XPEventRepository _xPEventRepository;
        private readonly UserStatsRepository _userStatsRepository;
        private readonly IXpCalculationStrategy _xpStrategy;

        public XPService(XPEventRepository xPEventRepository, UserStatsRepository userStatsRepository, IXpCalculationStrategy xpStrategy)
        {
            _xPEventRepository = xPEventRepository;
            _userStatsRepository = userStatsRepository;
            _xpStrategy = xpStrategy;
        }

        public int CalculateXP(Task task, int streakCount)
        {
            return _xpStrategy.CalculateXP(task, streakCount);
        }

        public int AwardXP(int userId, Task task, int awardXp, RewardReason reason)
        {
            XpEvent xpEvent = new XpEvent(userId, awardXp, reason, task.Id);
            _xPEventRepository.Insert(xpEvent);

            UserStats? userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            userStats.AddXp(awardXp);
            _userStatsRepository.UpdateTotalXp(userId, userStats.TotalXp);

            return awardXp;
        }

        public int AwardXP(Task task, int awardXp, RewardReason reason, SqliteConnection connection, SqliteTransaction transaction)
        {
            XpEvent xpEvent = new(task.UserId, awardXp, reason, task.Id);
            _xPEventRepository.Insert(xpEvent, connection, transaction);

            UserStats? userStats = _userStatsRepository.GetByUserId(task.UserId, connection, transaction) ?? throw new UserStatsNotFoundException(task.UserId);
            userStats.AddXp(awardXp);
            _userStatsRepository.UpdateTotalXp(task.UserId, userStats.TotalXp, connection, transaction);

            return awardXp;
        }

        public int GetTotalXP(int userId)
        {
            UserStats? userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            return userStats.TotalXp;
        }

        public bool HasXPEventForTask(int taskId, RewardReason reason)
        {
            var xpEvent = _xPEventRepository.GetAllByTaskId(taskId);
            return xpEvent.Any(e => e.Reason == reason);
        }
    }
}