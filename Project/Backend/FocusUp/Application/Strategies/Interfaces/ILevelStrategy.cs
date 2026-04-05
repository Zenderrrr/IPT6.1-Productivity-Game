using System;

namespace FocusUp.Application.Strategies.Interfaces
{
    public interface ILevelStrategy
    {
        int CalculateLevel(int totalXP);
        double CalculateProgressToNextLevel(int totalXP);
    }
}