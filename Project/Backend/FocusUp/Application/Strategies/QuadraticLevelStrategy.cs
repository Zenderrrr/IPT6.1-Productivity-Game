using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Strategies
{
    public class QuadraticLevelStrategy : ILevelStrategy
    {
        /// <summary>
        /// Scale factor which is used to calculate levels - higher more xp to level up
        /// </summary>
        private readonly int _baseXP;

        public QuadraticLevelStrategy(int baseXP = 100) => _baseXP = baseXP;

        // calculate the current Level based on the totalXP
        public int CalculateLevel(int totalXP)
        {
            return (int)Math.Floor(Math.Sqrt((double)totalXP / _baseXP)) + 1;
        }

        // calculate the progress to next Level (value within 0 to 1)
        public double CalculateProgressToNextLevel(int totalXP)
        {
            int level = CalculateLevel(totalXP);

            double xpCurrent = _baseXP * Math.Pow((level - 1), 2);
            double xpNext = _baseXP * Math.Pow(level, 2);

            return (totalXP - xpCurrent) / (xpNext - xpCurrent);
        }
    }
}