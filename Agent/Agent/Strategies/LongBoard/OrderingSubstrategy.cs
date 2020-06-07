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
            var sortedTeam = new List<int>(gameInfo.AlliesIds);
            sortedTeam.Add(gameInfo.AgentId);
            sortedTeam.Remove(gameInfo.LeaderId);
            sortedTeam.Sort();
            sortedTeam.Add(gameInfo.LeaderId);
            int myIndex = sortedTeam.IndexOf(gameInfo.AgentId);
            if (myIndex != 0) board.neighborIds.nearGoal = sortedTeam[myIndex - 1];
            if (myIndex != sortedTeam.Count - 1) board.neighborIds.nearFront = sortedTeam[myIndex + 1];
            List<int> subareasLengths = GetSubareasLengths(gameInfo);
            board.MyBounds = GetMyBounds(gameInfo, subareasLengths, myIndex);

            board.MySubareaFields = new CommonBoard.PosField[gameInfo.BoardSize.X.Value, board.MyAreaSize];
            for(int i = 0; i < gameInfo.BoardSize.X.Value; i++)
            {
                for(int j = 0; j < board.MyAreaSize; j++)
                {
                    board.MySubareaFields[i, j] = new CommonBoard.PosField(i, board.MyBounds.Min + j);
                }
            }
            if(_board.Type != CommonBoard.AgentType.Goalie)
            {
                for (int i = 0; i < gameInfo.BoardSize.X.Value; i++)
                {
                    board.FieldsToPlaceOn[i] = new CommonBoard.PosField(i, 
                        (board.Team == Team.Blue ? board.MyBounds.Min - 1 : board.MyBounds.Max + 1));
                }
            }
            if (_board.Type != CommonBoard.AgentType.Leader)
            {
                for (int i = 0; i < gameInfo.BoardSize.X.Value; i++)
                {
                    board.FieldsToTakeFrom[i] = board.GetFieldAt(new Point(i,
                        (board.Team == Team.Red ? board.MyBounds.Min : board.MyBounds.Max)));
                }
            }
            //Log.Debug("I'm at {@position} and I should be between {@bounds}",
            //    gameInfo.Position, board.MyBounds);
            //Log.Debug("I'm {agentId} and my neighbors are {neighborNearGoal} and {neighborNearFront}",
            //    gameInfo.AgentId, board.neighborIds.nearGoal, board.neighborIds.nearFront);

        }

        private (int Min, int Max) GetMyBounds(GameStarted gameInfo, List<int> subareasLengths, int myIndex)
        {
            int minBound, maxBound;
            if(_board.Team == Team.Blue)
            {
                minBound = 0;
                int i;
                for (i = 0; i < myIndex; i++)
                {
                    minBound += subareasLengths[i];
                }
                maxBound = minBound + subareasLengths[i] - 1;
            }
            else
            {
                maxBound = gameInfo.BoardSize.Y.Value - 1;
                int i;
                for (i = 0; i < myIndex; i++)
                {
                    maxBound -= subareasLengths[i];
                }
                minBound = maxBound - subareasLengths[i] + 1;
            }
            return (minBound, maxBound);
        }

        private List<int> GetSubareasLengths(GameStarted gameInfo)
        {
            int taskAreaSize = gameInfo.BoardSize.Y.Value - gameInfo.GoalAreaSize * 2;
            //fields on the edge of each agents subarea (the edge that's closer to the enemy)
            //will be where agent closer to the enemy will put his collected pieces
            //same for closest to goal area non goalie agent
            //so we have to subtract 1 from task area size as line of fields just next to goal area
            //will belong to goalie and should not be included in dividing among non goalie agents
            int nonGoalieAreaSize = taskAreaSize - 1;
            //task area is divided near equally among agents
            int subareaSize = gameInfo.AlliesIds.Count() ==0 ? 1 : nonGoalieAreaSize / gameInfo.AlliesIds.Count();
            //it usually isn't exactly equally divided
            //certain number (indicated by biggerSubareasCount) of agents in front will have +1 field
            int biggerSubareasCount = nonGoalieAreaSize - subareaSize * gameInfo.AlliesIds.Count();
            int smallerSubareasCount = gameInfo.AlliesIds.Count() - biggerSubareasCount;
            List<int> res = new List<int>
            {
                gameInfo.GoalAreaSize+1//goalie subarea
            };
            for (int i = 0; i < smallerSubareasCount; i++)
                res.Add(subareaSize);
            for (int i = 0; i < biggerSubareasCount; i++)
                res.Add(subareaSize + 1);
            Console.WriteLine("subareas count" + res.Count);
            return res;
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
