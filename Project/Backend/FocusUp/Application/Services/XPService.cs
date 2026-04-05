using FocusUp.Application.Strategies.Interfaces;
using System;

public class XPService
{
    private readonly XPEventRepository _eventRepository;
    private readonly UserStatsRepository _userStatsRepository;
    private readonly IXpCalculationStrategy _xpStrategy;

    public XPService(XPEventRepository xPEventRepository, UserStatsRepository userStatsRepository, IXpCalculationStrategy xpStrategy )
    {
        _eventRepository = xPEventRepository;
        _userStatsRepository = userStatsRepository;
        _xpStrategy = xpStrategy;
    }

    public int CalculateXP(Task task, int streakCount)
    {
        throw new NotImplementedException();
    }

    public int AwardXP(int userId, Task task, int streakCount, string reason)
    {
        throw new NotImplementedException();
    }

    public int GetTotalXP(int userId)
    {
        throw new NotImplementedException();
    }

    public bool HasXPEventForTask(int taskId, string reason)
    {
        throw new NotImplementedException();
    }
}