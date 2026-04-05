using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Strategies
{
    public class DefaultXpCalculationStrategy : IXpCalculationStrategy
    {
        private double _streakBonusFactor = 0.02;
        private double _temporaryBonusFactor = 0.05;

        public DefaultXpCalculationStrategy()
        {
        }

        public DefaultXpCalculationStrategy(double streakBonusFactor, double temporaryBonusFactor)
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

        public double GetBonusMultiplier(int streakCount)
        {
            return streakCount * _streakBonusFactor;
        }
    }
}