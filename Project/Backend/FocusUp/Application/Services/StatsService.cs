using FocusUp.Application.DTOs;
using FocusUp.Infrastructure.Repositories;
using System;
using FocusUp.Common.Exceptions;

namespace FocusUp.Application.Services
{
    public class StatsService
    {
        private readonly TaskRepository _taskRepository;
        private readonly XPEventRepository _xpEventRepository;
        private readonly UserStatsRepository _userStatsRepository;

        public StatsService(TaskRepository taskRepository, XPEventRepository xPEventRepository, UserStatsRepository userStatsRepository)
        {
            _taskRepository = taskRepository;
            _xpEventRepository = xPEventRepository;
            _userStatsRepository = userStatsRepository;
        }

        public StatsDto GetStats(int userId, int? rangeInDays = null, DateTime? from = null, DateTime? to = null)
        {
            (DateTime from, DateTime to) period = ResolvePeriod(rangeInDays, from, to);

            var userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);

            var statsDto = new StatsDto();

            // zeitabhängige Werte
            statsDto.TasksDone = _taskRepository.GetCompletedTasksPerDay(userId, period.from, period.to).Sum(t => t.count);
            statsDto.TotalXp = _xpEventRepository.GetXpPerDay(userId, period.from, period.to).Sum(t => t.xp);
            statsDto.TotalTimeMin = _taskRepository.SumDurationByPeriod(userId, period.from, period.to);

            // globale Werte
            statsDto.BestStreak = userStats.BestStreak;
            statsDto.StreakCount = userStats.StreakCount;
            statsDto.TasksOpen = userStats.TasksOpen;

            return statsDto;
        }

        public (DateTime from, DateTime to) ResolvePeriod(int? rangeInDays = null, DateTime? from = null, DateTime? to = null)
        {
            if (rangeInDays != null)
            {
                DateTime today = DateTime.Today;

                DateTime beginPeriod = today.AddDays(-rangeInDays.Value);
                DateTime endPeriod = today.AddDays(1).AddTicks(-1);
                return (beginPeriod, endPeriod);
            }
            else
            {
                if (from == null || to == null) throw new ArgumentException("Range or from/to must be provided.");
                return (from.Value.Date, to.Value.Date.AddDays(1).AddTicks(-1));
            }
        }

        public List<ProductivityDto> GetProductivity(int userId, int? rangeInDays = null, DateTime? from = null, DateTime? to = null)
        {
            (DateTime from, DateTime to) period = ResolvePeriod(rangeInDays, from, to);

            var productivityDtos = new List<ProductivityDto>();

            var completedTaskPerDay = _taskRepository.GetCompletedTasksPerDay(userId, period.from, period.to).ToDictionary(x => x.date.Date, x => x.count);
            var xpPerDay = _xpEventRepository.GetXpPerDay(userId, period.from, period.to).ToDictionary(x => x.date.Date, x => x.xp);
            var durationPerDay = _taskRepository.SumDurationPerDay(userId, period.from, period.to).ToDictionary(x => x.date.Date, x => x.durationMin);

            for (int i = 0; i <= ((period.to.Date - period.from.Date).Days); i++)
            {
                var prodDto = new ProductivityDto();

                DateTime currDate = period.from.AddDays(i);
                prodDto.Date = period.from.AddDays(i);

                completedTaskPerDay.TryGetValue(currDate, out int completedTasks);
                prodDto.CompletedTasks = completedTasks;

                xpPerDay.TryGetValue(currDate, out int xp);
                prodDto.XPGained = xp;

                durationPerDay.TryGetValue(currDate, out int durationMin);
                prodDto.TimeSpent = durationMin;

                productivityDtos.Add(prodDto);
            }
            return productivityDtos;
        }
    }
}