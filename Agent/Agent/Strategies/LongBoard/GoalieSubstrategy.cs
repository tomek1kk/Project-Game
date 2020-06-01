using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using static Agent.Strategies.LongBoard.StrategyHelpers;

namespace Agent.Strategies.LongBoard
{
    public class GoalieSubstrategy : ISubStrategy
    {
        private CommonBoard _board;
        private bool _exchageRequestOnTheLine = false;
        private string _patrolDirection = "E";
        public GoalieSubstrategy(CommonBoard board)
        {
            _board = board;
        }
        public bool IsDone(AgentInfo agentInfo)
        {
            return false;
        }

        public Message MakeDecision(AgentInfo agent)
        {
            if(_board.MoveError)
            {
                _board.MoveError = false;
                return GetRandomMove("W", "E", _board.Team == Team.Blue ? "S" : "N");
            }
            if(agent.HasPiece)
            {
                Point undiscoveredGoal = FindUndiscoveredGoalCoordinates(agent.Position);
                if (undiscoveredGoal == agent.Position)
                    return new Message<PutPieceRequest>(new PutPieceRequest());
                return GetMoveTo(agent.Position, undiscoveredGoal);
            }
            if(_board.GetFieldAt(agent.Position)?.DistToPiece == 0)
            {
                return new Message<PickPieceRequest>(new PickPieceRequest());
            }
            CommonBoard.PosField fieldToTakeFrom;
            if(!((fieldToTakeFrom = FindFieldToTakeFrom(agent)) is null))
            {
                return GetMoveTo(agent.Position, fieldToTakeFrom.Position);
            }
            else if(!_exchageRequestOnTheLine)
            {
                _exchageRequestOnTheLine = true;
                return new Message<ExchangeInformationRequest>(
                    new ExchangeInformationRequest
                    {
                        AskedAgentId = _board.neighborIds.nearFront
                    });
            }
            else
            {
                if(_board.IsFieldToTakeFrom(agent.Position))
                {
                    if(_patrolDirection == "E" && agent.Position.X == _board.FieldsToTakeFrom.Length - 1)
                    {
                        _patrolDirection = "W";
                    }
                    else if (_patrolDirection == "W" && agent.Position.X == 0)
                    {
                        _patrolDirection = "E";
                    }
                    return new Message<MoveRequest>(new MoveRequest { Direction = _patrolDirection });
                }
                else
                {
                    return GetMoveTo(agent.Position, _board.FieldsToTakeFrom[agent.Position.X].Position);
                }
            }
        }
        public Point FindUndiscoveredGoalCoordinates(Point currentPosition)
        {
            if (Team.Red == _board.Team)
            {
                return SearchNearestGoalOnRed(currentPosition);
            }
            else
            {
                return SearchNearestGoalOnBlue(currentPosition);
            }
            throw new Exception("All goals should be realized.");
        }
        private Point SearchNearestGoalOnRed(Point currentPosition)
        {
            if (_board.IsFieldToTakeFrom(currentPosition)) currentPosition.Y++;
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(currentPosition);
            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                if (!_board.IsInMyArea(current))
                    continue;
                if (_board.GetFieldAt(current).goalInfo == GoalInfo.IDK)
                    return current;
                queue.Enqueue(new Point(current.X, current.Y + 1));
                queue.Enqueue(new Point(current.X - 1, current.Y));
                queue.Enqueue(new Point(current.X + 1, current.Y));
                queue.Enqueue(new Point(current.X - 1, current.Y + 1));
                queue.Enqueue(new Point(current.X + 1, current.Y + 1));
            }
            throw new Exception("All goals are discovered");
        }

        private Point SearchNearestGoalOnBlue(Point currentPosition)
        {
            if (_board.IsFieldToTakeFrom(currentPosition)) currentPosition.Y--;
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(currentPosition);
            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                if (!_board.IsInMyArea(current))
                    continue;
                if (_board.GetFieldAt(current).goalInfo == GoalInfo.IDK)
                    return current;
                queue.Enqueue(new Point(current.X, current.Y - 1));
                queue.Enqueue(new Point(current.X - 1, current.Y));
                queue.Enqueue(new Point(current.X + 1, current.Y));
                queue.Enqueue(new Point(current.X - 1, current.Y - 1));
                queue.Enqueue(new Point(current.X + 1, current.Y - 1));
            }
            throw new Exception("All goals are discovered");
        }

        private CommonBoard.PosField FindFieldToTakeFrom(AgentInfo agent)
        {
            return _board.FieldsToTakeFrom
                .Where(f => f.DistToPiece == 0)
                .OrderBy(f => Dist(f.Position, agent.Position))
                .FirstOrDefault();  
        }

        public void UpdateMap(Message message, Point position)
        {
            switch(message.MessageId)
            {
                case MessageType.ExchangeInformationGMResponse:
                    _exchageRequestOnTheLine = false;
                    _board.UpdateDistances((ExchangeInformationGMResponse)message.GetPayload());
                    break;
                case MessageType.PutPieceResponse:
                    PutPieceResponseHandler((PutPieceResponse)message.GetPayload(), position);
                    break;
                case MessageType.MoveResponse:
                    MoveResponseHandler((MoveResponse)message.GetPayload(), position);
                    break;
                case MessageType.MoveError:
                    MoveErrorResponseHandler((MoveError)message.GetPayload());
                    break;
                case MessageType.PickPieceError:
                    PickPieceErrorResponseHandler((PickPieceError)message.GetPayload(), position);
                    break;
            }
        }

        private void MoveResponseHandler(MoveResponse moveResponse, Point position)
        {
            if (_board.GetFieldAt(position) is null) return;
            _board.GetFieldAt(position).DistToPiece = moveResponse.ClosestPiece ?? Int32.MaxValue;
        }

        private void PickPieceErrorResponseHandler(PickPieceError pickPieceError, Point position)
        {
            _board.GetFieldAt(position).DistToPiece = Int32.MaxValue;
        }

        private void MoveErrorResponseHandler(MoveError moveError)
        {
            _board.MoveError = true;
        }

        private void PutPieceResponseHandler(PutPieceResponse putPieceRespone, Point position)
        {
            switch (putPieceRespone.PutResult)
            {
                case PutResultEnum.NormalOnGoalField:
                    _board.GetFieldAt(position).goalInfo = GoalInfo.DiscoveredGoal;
                    break;
                case PutResultEnum.NormalOnNonGoalField:
                    _board.GetFieldAt(position).goalInfo = GoalInfo.DiscoveredNotGoal;
                    break;
                case PutResultEnum.TaskField:
                case PutResultEnum.ShamOnGoalArea:
                    //TaskField- odłożynie na pole które nie jeste goalem (środek planszy)
                    //ShamOnGoalArea-nie wiemy czy pole pod było prawdzym goalem czy nie
                    break;
            }
        }
    }
}
