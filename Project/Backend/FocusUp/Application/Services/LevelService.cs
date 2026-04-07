using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Services
{
    public class LevelService
    {
        private readonly ILevelStrategy _levelStrategy;

        public LevelService(ILevelStrategy levelStrategy)
        {
            _levelStrategy = levelStrategy;
        }

        public int GetLevel(int totalXP)
        {
            throw new NotImplementedException();
        }

        public double GetProgressToNextLevel(int totalXP)
        {
            throw new NotImplementedException();
        }

        public int GetXpForNextLevel(int totalXP)
        {
            throw new NotImplementedException();
        }
    }
}