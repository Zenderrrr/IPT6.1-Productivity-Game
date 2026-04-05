using System;
public interface IXpCalculationStrategy
{
    int CalculateXP(Task task, int streakCount);
    double GetBonusMultiplier(int streakCount);
}