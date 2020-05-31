using CommunicationLibrary;
using CommunicationLibrary.Information;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Agent.Strategies.LongBoard
{
    public class MasterStrategy : IStrategy
    {
        private List<ISubStrategy> _substrategies = new List<ISubStrategy>();
        private int _currentSubstrategyId = 0;
        private CommonBoard _board;
        private ISubStrategy currentSubstrategy => 
            _currentSubstrategyId < _substrategies.Count
            ? _substrategies[_currentSubstrategyId]
            : null;
        public MasterStrategy(GameStarted gameInfo)
        {
            _board = new CommonBoard();
            _substrategies.Add(new OrderingSubstrategy(gameInfo, _board));
        }
        public Message MakeDecision(AgentInfo agent)
        {
            while (currentSubstrategy?.IsDone(agent) ?? false) _currentSubstrategyId++;
            if (currentSubstrategy is null) Thread.Sleep(100000);
            return currentSubstrategy.MakeDecision(agent);
        }

        public void UpdateMap(Message message, Point position)
        {
            currentSubstrategy.UpdateMap(message, position);
        }
    }
}
