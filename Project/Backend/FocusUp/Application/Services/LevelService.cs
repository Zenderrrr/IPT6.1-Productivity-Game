using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Services
{
    public class LevelService
    {
        private readonly ILevelStrategy _levelStrategy;

        public LevelService(ILevelStrategy levelStrategy) => _levelStrategy = levelStrategy;

        public int GetLevel(int totalXP) => _levelStrategy.CalculateLevel(totalXP);

        public double GetProgressToNextLevel(int totalXP) => _levelStrategy.CalculateProgressToNextLevel(totalXP);

        public int GetXpForNextLevel(int totalXP)
        {
            int level = GetLevel(totalXP);
            return 100 * (int)Math.Pow((level - 1), 2);
        }

        public int GetXpCurrent(int totalXP)
        {
            int level = GetLevel(totalXP);
            return 100 * (int)Math.Pow(level, 2);
        }
    }
}