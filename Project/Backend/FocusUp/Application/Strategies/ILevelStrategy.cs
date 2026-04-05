using System;

public interface ILevelStrategy
{
    int CalculateLevel(int totalXP);
    double CalculateProgressToNextLevel(int totalXP);
}