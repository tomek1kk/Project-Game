using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using GameMaster.Configuration;
using GameMaster.Game;
using System;

namespace GameMaster.MessageHandlers
{
    public class ExchangeInformationRequestHandler : MessageHandler
    {
        private int _respondent;
        private bool _noRespondentOnMap;
        protected override void ClearHandler() { }
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }
        protected override bool CheckRequest(Map map)
        {
            if (map.GetPlayerById(_respondent) == null)
            {
                _noRespondentOnMap = true;
                return false;
            }
            else
            {
                _noRespondentOnMap = false;
                return true;
            }
        }

        protected override void Execute(Map map)
        {
            map.SaveInformationExchange(_agentId, _respondent);
        }

        protected override Message GetResponse(Map map)
        {
            if(_noRespondentOnMap)
            {
                return new Message<NotDefinedError>()
                {
                    AgentId = _agentId,
                    MessagePayload = new NotDefinedError()
                    {
                        Position = (Position)map.GetPlayerById(_agentId).Position,
                        HoldingPiece = map.GetPlayerById(_agentId).IsHolding
                    }
                };
            }
            else
            {
                return new Message<RedirectedExchangeInformationRequest>()
                {
                    AgentId = _respondent,
                    MessagePayload = new RedirectedExchangeInformationRequest()
                    {
                        AskingId = _agentId,
                        Leader = map.GetPlayerById(_agentId).IsLeader,
                        TeamId = map.GetPlayerById(_agentId).Team == Team.Red ? "red" : "blue"
                    }
                };
            }
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            _respondent = (int)((ExchangeInformationRequest)payload).AskedAgentId;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.AskPenalty));
        }
    }
}
