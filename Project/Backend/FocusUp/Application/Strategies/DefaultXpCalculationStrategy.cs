using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Strategies
{
    public class DefaultXpCalculationStrategy : IXpCalculationStrategy
    {
        private readonly double _streakBonusFactor;
        private readonly double _temporaryBonusFactor;

        public DefaultXpCalculationStrategy(double streakBonusFactor = 0.02, double temporaryBonusFactor = 0.05)
        {
            _streakBonusFactor = streakBonusFactor;
            _temporaryBonusFactor = temporaryBonusFactor;
        }

        public int CalculateXP(Task task, int streakCount)
        {
            double baseXP = task.Difficulty;
            double timeBonus = task.DurationMin / 3;
            double streakBonus = (baseXP + timeBonus) * GetBonusMultiplier(streakCount);
            double temporaryBonus = (baseXP + timeBonus + streakBonus) * _temporaryBonusFactor;

            return (int)Math.Floor(baseXP + timeBonus + streakBonus + temporaryBonus);
        }

        public double GetBonusMultiplier(int streakCount) => streakCount * _streakBonusFactor;
    }
}