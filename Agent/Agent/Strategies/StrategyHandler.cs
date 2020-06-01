using CommunicationLibrary.Information;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Strategies
{
    public enum StrategyType
    {
        SampleStrategy = 1,
        LongBoardStrategy = 2,
    }

    public class StrategyHandler
    {
        private readonly Dictionary<StrategyType, IStrategy> handlers;
        public StrategyHandler(GameStarted gameInfo)
        {
            handlers = new Dictionary<StrategyType, IStrategy>()
            {
                { StrategyType.SampleStrategy, new SampleStrategy(
                    gameInfo.BoardSize.X.Value, gameInfo.BoardSize.Y.Value, gameInfo.TeamId, gameInfo.GoalAreaSize)},
                {StrategyType.LongBoardStrategy, new LongBoard.MasterStrategy(gameInfo) }
            };
        }
        public IStrategy GetStrategy(StrategyType t) => handlers[t];
    }
}
