using CommunicationLibrary;
using CommunicationLibrary.Information;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            CommonBoard.AgentType agentType;
            if (gameInfo.AlliesIds.Count() == 0) return;
            if (gameInfo.AlliesIds.Count() == 1)
            {
                agentType =
                gameInfo.LeaderId == gameInfo.AgentId ? CommonBoard.AgentType.Leader
                : CommonBoard.AgentType.Goalie;
            }
            else
            {
                agentType =
                    gameInfo.LeaderId == gameInfo.AgentId ? CommonBoard.AgentType.Leader
                    :
                    (
                    gameInfo.AlliesIds
                    .Where(allyId => allyId != gameInfo.LeaderId)
                    .Min() > gameInfo.AgentId
                    ? CommonBoard.AgentType.Goalie
                    : CommonBoard.AgentType.Standard
                    );
            }
            _board = new CommonBoard(gameInfo, agentType);
            _substrategies.Add(new OrderingSubstrategy(gameInfo, _board));
            switch (_board.Type)
            {
                case CommonBoard.AgentType.Standard:
                    _substrategies.Add(new NormalSubstrategy(_board));
                    break;
                case CommonBoard.AgentType.Goalie:
                    _substrategies.Add(new GoalieSubstrategy(_board));
                    break;
                case CommonBoard.AgentType.Leader:
                    _substrategies.Add(new LeaderSubstrategy(_board));
                    break;
            }
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
