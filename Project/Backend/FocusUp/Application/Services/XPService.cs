using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

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

        public int AwardXP(int userId, Task task, int streakCount, RewardReason reason)
        {
            int awardedXp = CalculateXP(task, streakCount);
            XpEvent xpEvent = new XpEvent(userId, awardedXp, reason, task.Id);
            _xPEventRepository.Insert(xpEvent);

            UserStats? userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            userStats.AddXp(awardedXp);
            _userStatsRepository.UpdateTotalXp(userId, userStats.TotalXp);

            return awardedXp;
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