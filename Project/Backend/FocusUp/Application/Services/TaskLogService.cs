using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using System.Collections.Generic;
using System;

namespace FocusUp.Application.Services
{
    public class TaskLogService
    {
        private readonly TaskLogRepository _taskLogRepository;

        public TaskLogService(TaskLogRepository taskLogRepository) => _taskLogRepository = taskLogRepository;

        public int CreateLog(int userId, int taskId, RewardReason action, int xpAwarded)
        {
            var taskLog = new TaskLog(userId, taskId, action, xpAwarded);
            return _taskLogRepository.Insert(taskLog);
        }

        public List<TaskLog> GetLogsByUserId(int userId) => _taskLogRepository.GetAllByUserId(userId);

        public List<TaskLog> GetLogsByTaskId(int taskId) => _taskLogRepository.GetAllByTaskId(taskId);

        public List<TaskLog> GetRecentLogs(int userId, int limit) => _taskLogRepository.GetRecentByUserId(userId, limit);
    }
}