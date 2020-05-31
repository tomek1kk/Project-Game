using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Information;
using CommunicationLibrary.Request;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Agent.Strategies.LongBoard
{
    class OrderingSubstrategy : ISubStrategy
    {
        private CommonBoard _board;
        private Random _rng = new Random();
        public OrderingSubstrategy(GameStarted gameInfo, CommonBoard board)
        {
            _board = board;
            int taskAreaSize = gameInfo.BoardSize.Y.Value - gameInfo.GoalAreaSize * 2;
            //fields on the edge of each agents subarea (the edge that's closer to the enemy)
            //will be where agent closer to the enemy will put his collected pieces
            //same for closest to goal area non goalie agent
            //so we have to subtract 1 from task area size as line of fields just next to goal area
            //will belong to goalie and should not be included in dividing among non goalie agents
            int nonGoalieAreaSize = taskAreaSize - 1;
            //task area is divided near equally among agents
            int subareaSize = nonGoalieAreaSize / gameInfo.AlliesIds.Count();
            //it usually isn't exactly equally divided
            //certain number (indicated by biggerSubareasCount) of agents in front will have +1 field
            int biggerSubareasCount = nonGoalieAreaSize - subareaSize * gameInfo.AlliesIds.Count();
            (int myAreaStart, int myAreaSize)
                = GetSubareaForAgent(gameInfo, subareaSize, biggerSubareasCount);
            board.MyBounds = (myAreaStart, myAreaStart + myAreaSize - 1);
            var sortedAllies = new List<int>(gameInfo.AlliesIds);
            sortedAllies.Sort();
            board.neighborIds =
                (
                board.AmLeader
                ? sortedAllies.Max()
                : sortedAllies
                    .Where(allyId => allyId != gameInfo.LeaderId)
                    .Where(allyId => allyId < gameInfo.AgentId)
                    .Cast<int?>()
                    .LastOrDefault(),//null if agent is goalie
                board.AmLeader
                ? null
                : sortedAllies
                    .Where(allyId => allyId != gameInfo.LeaderId)
                    .Where(allyId => allyId > gameInfo.AgentId)
                    .Append(gameInfo.LeaderId)//moved leader to end
                    .Cast<int?>()
                    .FirstOrDefault()
                );
            Log.Debug("I'm at {@position} and I should be between {@bounds}",
                gameInfo.Position, board.MyBounds);
            Log.Debug("I'm {agentId} and my neighbors are {neighborNearGoal} and {neighborNearFront}",
                gameInfo.AgentId, board.neighborIds.nearGoal, board.neighborIds.nearFront);

        }

        private (int myAreaStart, int myAreaSize) GetSubareaForAgent(
            GameStarted gameInfo, int subareaSize, int biggerSubareasCount)
        {
            //leader always in front so he will never ask for information exchange
            if (gameInfo.LeaderId == gameInfo.AgentId)
                return GetSubareaForLeader(gameInfo, subareaSize, biggerSubareasCount);
            else
            {
                //count allies which will be closer to our goal area than me
                int fartherFromFrontCount = gameInfo.AlliesIds
                    .Where(allyId => allyId != gameInfo.LeaderId)
                    .Where(allyId => allyId < gameInfo.AgentId)
                    .Count();
                //non-leader agent with lowest id will be goalie
                if (fartherFromFrontCount == 0)
                    return GetSubareaForGoalie(gameInfo);
                else
                    return GetSubareaForNormal(gameInfo, subareaSize, biggerSubareasCount, fartherFromFrontCount);
            }
        }

        private (int myAreaStart, int myAreaSize) GetSubareaForLeader(
            GameStarted gameInfo, int subareaSize, int biggerSubareasCount)
        {
            int myAreaSize = biggerSubareasCount > 0 ? subareaSize + 1 : subareaSize;
            int myAreaStart = gameInfo.TeamId == "blue"
                ? gameInfo.BoardSize.Y.Value - gameInfo.GoalAreaSize - myAreaSize
                : gameInfo.GoalAreaSize;
            _board.AmLeader = true;
            return (myAreaStart, myAreaSize);
        }

        private (int myAreaStart, int myAreaSize) GetSubareaForGoalie(
            GameStarted gameInfo)
        {
            //also include fields just in front of goal area
            int myAreaSize = gameInfo.GoalAreaSize + 1;
            int myAreaStart = gameInfo.TeamId == "red"
                ? gameInfo.BoardSize.Y.Value - gameInfo.GoalAreaSize - 1
                : 0;
            _board.AmGoalie = true;
            return (myAreaStart, myAreaSize);
        }

        private (int myAreaStart, int myAreaSize) GetSubareaForNormal(
            GameStarted gameInfo, int subareaSize, int biggerSubareasCount, int fartherFromFrontCount)
        {
            int closerToFrontCount = gameInfo.AlliesIds.Count() - fartherFromFrontCount;
            //there are more +1 subareas than agents in front of me, so I have bigger subarea too
            int myAreaSize = biggerSubareasCount > closerToFrontCount
                ? subareaSize + 1
                : subareaSize;
            int biggerSubareasInFrontOfMeCount = Math.Min(biggerSubareasCount, closerToFrontCount);
            int normalSubareasInFrontOfMeCount = closerToFrontCount - biggerSubareasCount;
            int tilesInFrontOfMe =
                gameInfo.GoalAreaSize +
                biggerSubareasInFrontOfMeCount * (subareaSize + 1) +
                normalSubareasInFrontOfMeCount * subareaSize;
            int myAreaStart = gameInfo.TeamId == "blue"
                ? gameInfo.BoardSize.Y.Value - tilesInFrontOfMe - myAreaSize
                : tilesInFrontOfMe;
            return (myAreaStart, myAreaSize);
        }

        public bool IsDone(AgentInfo agentInfo)
        {
            return _board.MyBounds.Min <= agentInfo.Position.Y && agentInfo.Position.Y <= _board.MyBounds.Max;
        }

        public Message MakeDecision(AgentInfo agent)
        {
            if(_board.MoveError)
            {
                _board.MoveError = false;
                return new Message<MoveRequest>(new MoveRequest
                {
                    Direction = _rng.Next() % 2 == 0 ? "E" : "W"
                });
            }
            if(agent.Position.Y < _board.MyBounds.Min)
            {
                return new Message<MoveRequest>(new MoveRequest
                {
                    Direction = "N"
                });
            }
            else
            {
                return new Message<MoveRequest>(new MoveRequest
                {
                    Direction = "S"
                });
            }
        }

        public void UpdateMap(Message message, Point position)
        {
            if (message.MessageId == MessageType.MoveError) _board.MoveError = true;
        }
    }
}
