using System;

namespace FocusUp.Application.Strategies.Interfaces
{
    public interface IXpCalculationStrategy
    {
        int CalculateXP(Task task, int streakCount);
        double GetBonusMultiplier(int streakCount);
    }
}