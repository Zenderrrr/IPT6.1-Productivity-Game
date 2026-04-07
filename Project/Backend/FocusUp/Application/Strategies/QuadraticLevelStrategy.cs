using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Strategies
{
    public class QuadraticLevelStrategy : ILevelStrategy
    {
        private readonly int _baseXP;

        public QuadraticLevelStrategy(int baseXP = 100) => _baseXP = baseXP;

        // calculate the current Level based on the totalXP
        public int CalculateLevel(int totalXP)
        {
            return (int)Math.Floor(Math.Sqrt(totalXP / _baseXP)) + 1;
        }

        // calculate the progress to next Level (value within 0 to 1)
        public double CalculateProgressToNextLevel(int totalXP)
        {
            double xpCurrent = _baseXP * Math.Pow((CalculateLevel(totalXP) - 1), 2);
            double xpNext = _baseXP * Math.Pow(CalculateLevel(totalXP), 2);

            return (totalXP - xpCurrent) / (xpNext - xpCurrent);
        }
    }
}