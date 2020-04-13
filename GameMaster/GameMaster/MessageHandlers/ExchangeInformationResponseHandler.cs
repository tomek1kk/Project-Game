using CommunicationLibrary;
using CommunicationLibrary.Model;
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
    public class ExchangeInformationResponseHandler : MessageHandler
    {
        private int _requester;
        private bool _noExchangeSavedError;
        private ExchangeInformationResponse _message;
        protected override void ClearHandler() { }
        protected override void CheckAgentPenaltyIfNeeded(Map map) 
        {
            CheckIfAgentHasPenalty(map);    //or response may be immediate?
        }
        protected override bool CheckRequest(Map map)
        {
            if (map.ValidateAndRemoveInformationExchange(_requester, _agentId))
            {
                _noExchangeSavedError = false;
                return true;
            }
            else
            {
                _noExchangeSavedError = true;
                return false;
            }
        }

        protected override void Execute(Map map) { }

        protected override Message GetResponse(Map map)
        {
            if (_noExchangeSavedError)
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
            return new Message<ExchangeInformationGMResponse>()
            {
                AgentId = _requester,
                MessagePayload = new ExchangeInformationGMResponse()
                {
                    RespondToID = _message.RespondToID,
                    Distances = _message.Distances,
                    RedTeamGoalAreaInformations = _message.RedTeamGoalAreaInformations,
                    BlueTeamGoalAreaInformations = _message.BlueTeamGoalAreaInformations
                }
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            _requester = (int)((ExchangeInformationResponse)payload).RespondToID;
            _message = (ExchangeInformationResponse)payload;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.ResponsePenalty));
        }
    }
}
