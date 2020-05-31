using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using static Agent.Strategies.LongBoard.StrategyHelpers;

namespace Agent.Strategies.LongBoard
{
    public class LeaderSubstrategy : ISubStrategy
    {
        private CommonBoard _board;
        private int _patrolCounter = 0;
        private int _exchageResponseCountdown = 0;
        private string _patrolDir = "N";

        public LeaderSubstrategy(CommonBoard board)
        {
            _board = board;
        }

        public bool IsDone(AgentInfo agentInfo)
        {
            return false;
        }

        public Message MakeDecision(AgentInfo agent)
        {
            if (agent.ExchangeInfoRequests.Count > 0 && _exchageResponseCountdown <= 0)
            {
                var tmp = agent.ExchangeInfoRequests[0];
                agent.ExchangeInfoRequests.RemoveAt(0);
                if (tmp.TeamId == _board.Team.ToString().ToLower())
                {
                    _exchageResponseCountdown = 10;
                    return GenerateExchageResponse(agent, tmp);
                }
            }
            else
            {
                _exchageResponseCountdown--;
            }
            if (_board.MoveError)
            {
                _board.MoveError = false;
                return GetRandomMove("E", "W");
            }
            if (agent.HasPiece)
            {
                if(_board.IsFieldToPlaceOn(agent.Position))
                {
                    if(_board.GetFieldAt(agent.Position).DistToPiece == 0)
                    {
                        return GetRandomMove("E", "W");
                    }
                    return new Message<PutPieceRequest>(new PutPieceRequest());
                }
                return GetMoveTo(agent.Position, _board.FieldsToPlaceOn[agent.Position.X].Position);
            }
            Field agentField = _board.GetFieldAt(agent.Position);
            if(agentField.DistToPiece == 0 && !_board.IsFieldToPlaceOn(agent.Position))
            {
                return new Message<PickPieceRequest>(new PickPieceRequest());
            }
            if(Dist(_board.FieldsToPlaceOn[agent.Position.X].Position, agent.Position) < 2)
            {
                return new Message<MoveRequest>(new MoveRequest
                {
                    Direction = _board.Team == Team.Blue ? "N" : "S"
                });
            }
            return MakePatrolDecision(agent);
        }

        private Message GenerateExchageResponse(AgentInfo agent,
            RedirectedExchangeInformationRequest exchangeInformationRequest)
        {
            int[] distances = _board.FieldsToPlaceOn
                    .Select(f => f.DistToPiece)
                    .Concat(_board.FieldsToPlaceOn
                    .Select(f => (int)(f.LastUpdateDistToPiece.Ticks >> 32)))
                    .Concat(_board.FieldsToPlaceOn
                    .Select(f => (int)(f.LastUpdateDistToPiece.Ticks)))
                    .ToArray();
            return new Message<ExchangeInformationResponse>(new ExchangeInformationResponse
            {
                Distances = distances,
                RedTeamGoalAreaInformations = new string[0],
                BlueTeamGoalAreaInformations = new string[0],
                RespondToID = exchangeInformationRequest.AskingId
            });
        }

        private Message MakePatrolDecision(AgentInfo agent)
        {
            _patrolCounter++;
            if (_patrolCounter % 5 == 0)
            {
                return new Message<DiscoveryRequest>(new DiscoveryRequest());
            }
            if (_patrolCounter % 5 == 1)
            {
                return MoveIfPieceOnAdjacent(agent)
                    ?? MoveIfFoundDistanceCloserThanToPutLine(agent)
                    ?? MoveIfFoundDistanceCloserThanItsNeighbor(agent)
                    ?? ValidPatrolMove(agent);
            }
            return ValidPatrolMove(agent);
        }

        private Message MoveIfPieceOnAdjacent(AgentInfo agent)
        {
            for (int i = agent.Position.X - 1; i <= agent.Position.X + 1; i++)
            {
                for (int j = agent.Position.Y - 1; j <= agent.Position.Y + 1; j++)
                {
                    var p = new Point(i, j);
                    if (agent.Position == p || _board.GetFieldAt(p) is null) continue;
                    if (_board.GetFieldAt(p).DistToPiece == 0)
                        return GetMoveTo(agent.Position, p);
                }
            }
            return null;
        }
        private Message MoveIfFoundDistanceCloserThanToPutLine(AgentInfo agent)
        {
            for (int i = agent.Position.X - 1; i <= agent.Position.X + 1; i++)
            {
                for (int j = agent.Position.Y - 1; j <= agent.Position.Y + 1; j++)
                {
                    var p = new Point(i, j);
                    if (agent.Position == p || _board.GetFieldAt(p) is null) continue;
                    if (_board.GetFieldAt(p).DistToPiece < Dist(p, _board.FieldsToPlaceOn[i].Position))
                        return GetMoveTo(agent.Position, p);
                }
            }
            return null;
        }

        private Message MoveIfFoundDistanceCloserThanItsNeighbor(AgentInfo agent)
        {
            int minJ, maxJ, closerToPutLineOffset;
            switch (_board.Team)
            {
                case Team.Red:
                    minJ = agent.Position.Y; maxJ = agent.Position.Y + 1; closerToPutLineOffset = -1;
                    break;
                default:
                case Team.Blue:
                    minJ = agent.Position.Y - 1; maxJ = agent.Position.Y; closerToPutLineOffset = 1;
                    break;
            }
            for (int i = agent.Position.X - 1; i <= agent.Position.X + 1; i++)
            {
                for (int j = minJ; j <= maxJ; j++)
                {
                    var p = new Point(i, j);
                    if (agent.Position == p || _board.GetFieldAt(p) is null) continue;
                    if (_board.GetFieldAt(p).DistToPiece <
                        (_board.GetFieldAt(new Point(i, j + closerToPutLineOffset))?.DistToPiece ?? 0))
                        return GetMoveTo(agent.Position, p);
                }
            }
            return null;
        }

        private Message ValidPatrolMove(AgentInfo agent)
        {
            if (_patrolDir == "W" &&
                agent.Position.X == 0) _patrolDir = "N";
            if (_patrolDir == "N" &&
                agent.Position.Y <= _board.MyBounds.Min + 1) _patrolDir = "E";
            if (_patrolDir == "E" &&
                _board.FieldsToPlaceOn.Length - 1 - agent.Position.X == 0) _patrolDir = "S";
            if (_patrolDir == "S" &&
                agent.Position.Y >= _board.MyBounds.Max - 1) _patrolDir = "W";
            return new Message<MoveRequest>(new MoveRequest { Direction = _patrolDir });
        }

        public void UpdateMap(Message message, Point position)
        {
            foreach(Field field in _board.MySubareaFields)
            {
                if (field.LastUpdateDistToPiece < DateTime.Now.AddSeconds(-10))
                    field.DistToPiece = Int32.MaxValue;
            }
            foreach (Field field in _board.FieldsToPlaceOn)
            {
                if (field.LastUpdateDistToPiece < DateTime.Now.AddSeconds(-10))
                    field.DistToPiece = Int32.MaxValue;
            }
            switch (message.MessageId)
            {
                case MessageType.DiscoveryResponse:
                    _board.UpdateDistances((DiscoveryResponse)message.GetPayload(), position);
                    break;
                case MessageType.MoveResponse:
                    _board.UpdateDistance((MoveResponse)message.GetPayload(), position);
                    break;
                case MessageType.MoveError:
                    MoveErrorResponseHandler();
                    break;
                case MessageType.PickPieceError:
                    PickPieceErrorResponseHandler((PickPieceError)message.GetPayload(), position);
                    break;
            }
        }

        private void PickPieceErrorResponseHandler(PickPieceError pickPieceError, Point position)
        {
            _board.GetFieldAt(position).DistToPiece = Int32.MaxValue;
        }

        private void MoveErrorResponseHandler()
        {
            _board.MoveError = true;
        }
    }
}
