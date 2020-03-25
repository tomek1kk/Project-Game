using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Strategies
{
    public enum StrategyType
    {
        SampleStrategy = 1
    }

    public class StrategyHandler
    {
        private readonly Dictionary<StrategyType, Strategy> handlers;
        public StrategyHandler(int width, int height)
        {
            handlers = new Dictionary<StrategyType, Strategy>()
            {
                { StrategyType.SampleStrategy, new SampleStrategy(width, height)}
            };
        }
        public Strategy GetStrategy(StrategyType t) => handlers[t];
    }
}
