using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Response;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class DiscoveryRequestHandler : MessageHandler
    {
        private AbstractField _position;
        private int? _distanceFromCurrent;
        private int? _distanceN;
        private int? _distanceNE;
        private int? _distanceE;
        private int? _distanceSE;
        private int? _distanceS;
        private int? _distanceSW;
        private int? _distanceW;
        private int? _distanceNW;

        protected override void ClearHandler() { }
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }
        protected override bool CheckRequest(Map map)
        {
            return true;
        }

        protected override void Execute(Map map)
        {
            _position = map.GetPlayerById(_agentId).Position;
            _distanceFromCurrent = map.ClosestPieceForField(_position);
            _distanceN = map.IsInsideMap(_position.X, _position.Y + 1) ? (int?)map.ClosestPieceForField(map[_position.X, _position.Y + 1]) : null;
            _distanceNE = map.IsInsideMap(_position.X + 1, _position.Y + 1) ? (int?)map.ClosestPieceForField(map[_position.X + 1, _position.Y + 1]) : null;
            _distanceE = map.IsInsideMap(_position.X + 1, _position.Y) ? (int?)map.ClosestPieceForField(map[_position.X + 1, _position.Y]) : null;
            _distanceSE = map.IsInsideMap(_position.X + 1, _position.Y - 1) ? (int?)map.ClosestPieceForField(map[_position.X + 1, _position.Y - 1]) : null;
            _distanceS = map.IsInsideMap(_position.X, _position.Y - 1) ? (int?)map.ClosestPieceForField(map[_position.X, _position.Y - 1]) : null;
            _distanceSW = map.IsInsideMap(_position.X - 1, _position.Y - 1) ? (int?)map.ClosestPieceForField(map[_position.X - 1, _position.Y - 1]) : null;
            _distanceW = map.IsInsideMap(_position.X - 1, _position.Y) ? (int?)map.ClosestPieceForField(map[_position.X - 1, _position.Y]) : null;
            _distanceNW = map.IsInsideMap(_position.X - 1, _position.Y + 1) ? (int?)map.ClosestPieceForField(map[_position.X - 1, _position.Y + 1]) : null;
        }

        protected override Message GetResponse(Map map)
        {
            return new Message<DiscoveryResponse>()
            {
                MessagePayload = new DiscoveryResponse()
                {
                    DistanceFromCurrent = _distanceFromCurrent,
                    DistanceN = _distanceN,
                    DistanceNE = _distanceNE,
                    DistanceE = _distanceE,
                    DistanceSE = _distanceSE,
                    DistanceS = _distanceS,
                    DistanceSW = _distanceSW,
                    DistanceW = _distanceW,
                    DistanceNW = _distanceNW
                }
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.DiscoveryPenalty));
        }
    }
}
