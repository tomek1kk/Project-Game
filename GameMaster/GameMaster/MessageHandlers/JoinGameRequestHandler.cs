using CommunicationLibrary;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class JoinGameRequestHandler : MessageHandler
    {
        private bool _playerAlreadyOnMap = false;
        private Team _team;
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            return;
        }
        protected override bool CheckRequest(Map map)
        {
            _playerAlreadyOnMap = map.GetPlayerById(_agentId) != null;
            return !_playerAlreadyOnMap;
        }

        protected override void Execute(Map map)
        {
            map.AddPlayer(_team, _agentId);
        }

        protected override Message GetResponse(Map map)
        {
            if(_playerAlreadyOnMap)
            {
                return new Message<NotDefinedError>()
                {
                    AgentId = _agentId,
                    MessagePayload = new NotDefinedError()
                    {
                    }
                };
            }
            return new Message<JoinGameResponse>()
            {
                AgentId = _agentId,
                MessagePayload = new JoinGameResponse()
                {
                    AgentID = _agentId,
                    Accepted = true
                }
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            _team = TeamExtensions.ToTeam(((JoinGameRequest)payload).TeamId);
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.MinValue);
        }
    }
}
