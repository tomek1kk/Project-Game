using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class MoveRequestHandler : MessageHandler
    {
        private string _direction;
        private bool _errorMessage;
        private bool _moveError;
        private int _newX;
        private int _newY;

        protected override void ClearHandler()
        {
            _errorMessage = false;
            _moveError = false;
        }
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }
        protected override void ReadMessage(MessagePayload payload)
        {
            _direction = ((MoveRequest)payload).Direction;
        }

        protected override bool CheckRequest(Map map)
        {
            int x = map.GetPlayerById(_agentId).Position.X;
            int y = map.GetPlayerById(_agentId).Position.Y;
            switch (_direction)
            {
                case "N":
                    y++;
                    break;
                case "S":
                    y--;
                    break;
                case "W":
                    x--;
                    break;
                case "E":
                    x++;
                    break;
                default:
                    _errorMessage = true;
                    return false;
            }
            _newX = x; _newY = y;
            if (!map.IsInsideMap(x, y) ||
                (map.GetPlayerById(_agentId).Team == Team.Red && map.IsInsideBlueGoalArea(x, y)) ||
                (map.GetPlayerById(_agentId).Team == Team.Blue && map.IsInsideRedGoalArea(x, y)) ||
                map[x, y].IsOccupied)
                _moveError = true;
            return !_moveError;
        }

        protected override void Execute(Map map)
        {
            AbstractField oldField = map.GetPlayerById(_agentId).Position;
            AbstractField newField = map[_newX, _newY];
            oldField.MoveOut(map.GetPlayerById(_agentId));
            newField.MoveHere(map.GetPlayerById(_agentId));
            map.GetPlayerById(_agentId).Position = newField;
        }

        protected override Message GetResponse(Map map)
        {
            if (_errorMessage)
                return new Message<NotDefinedError>()
                {
                    MessagePayload = new NotDefinedError()
                    {
                        Position = (Position)map.GetPlayerById(_agentId).Position,
                        HoldingPiece = map.GetPlayerById(_agentId).IsHolding
                    }
                };
            else if (_moveError)
                return new Message<MoveError>()
                {
                    MessagePayload = new MoveError()
                    {
                        Position = (Position)map.GetPlayerById(_agentId).Position
                    }
                };
            return new Message<MoveResponse>()
            {
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = (Position)map.GetPlayerById(_agentId).Position,
                    ClosestPiece = map.ClosestPieceForField(map.GetPlayerById(_agentId).Position)
                }
            };
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.MovePenalty));
        }
    }
}
