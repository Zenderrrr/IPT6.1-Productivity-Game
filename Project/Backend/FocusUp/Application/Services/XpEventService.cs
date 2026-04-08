using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using System;

namespace FocusUp.Application.Services
{
    public class XpEventService
    {
        private readonly XPEventRepository _xPEventRepository;
        public XpEventService(XPEventRepository xPEventRepository) => _xPEventRepository = xPEventRepository;

        public int CreateXPEvent(int userId, int amount, RewardReason reason, int? taskId = null)
        {
            var xpEvent = new XpEvent(userId, amount, reason, taskId);
            return _xPEventRepository.Insert(xpEvent);
        }

        public List<XpEvent> GetXPEventsByUserId(int userId) => _xPEventRepository.GetAllByUserId(userId);

        public List<XpEvent> GetXPEventsByTaskId(int taskId) => _xPEventRepository.GetAllByTaskId(taskId);

        public List<XpEvent> GetRecentXPEvents(int userId, int limit) => _xPEventRepository.GetRecentByUserId(userId, limit);

        public int GetTotalXP(int userId) => _xPEventRepository.GetTotalXpByUserId(userId);
    }
}