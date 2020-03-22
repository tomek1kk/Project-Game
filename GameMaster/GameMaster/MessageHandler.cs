using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class MessageHandler
    {
        public void HandleMessage(Message message, IMessageSenderReceiver communicator, Map map)
        {
            if (message.GetPayload().ValidateMessage() == false || message.AgentId == null)
            {
                communicator.Send(new Message<NotDefinedError>()
                {
                    AgentId = message.AgentId,
                    MessagePayload = new NotDefinedError()
                });
                return;
            }

            Message response = GetResponseMessage(message, map);
            communicator.Send(response);
        }

        private Message GetResponseMessage(Message request, Map map)
        {
            int agentId = (int)request.AgentId;
            Message responseMessage = null;
            Player agent;

            switch (request.MessageId)
            {
                case MessageType.JoinGameRequest:
                    JoinGameRequest joinGameRequest = (JoinGameRequest)request.GetPayload();
                    JoinGameResponse response = map.AddPlayer(joinGameRequest.TeamId.ToTeam(), agentId);
                    responseMessage = new Message<JoinGameResponse>()
                    {
                        AgentId = agentId,
                        MessagePayload = response
                    };
                    break;
                case MessageType.CheckHoldedPieceRequest:
                    agent = map.GetPlayerById(agentId);
                    responseMessage = new Message<CheckHoldedPieceResponse>()
                    {
                        AgentId = agentId,
                        MessagePayload = agent.CheckHolding()
                    };
                    break;
                case MessageType.DestroyPieceRequest:
                    agent = map.GetPlayerById(agentId);
                    responseMessage = new Message<DestroyPieceResponse>()
                    {
                        AgentId = agentId,
                        MessagePayload = agent.DestroyHolding()
                    };
                    break;
                case MessageType.DiscoveryRequest:
                    responseMessage = new Message<DiscoveryResponse>()
                    {
                        AgentId = agentId,
                        MessagePayload = map.Discovery(agentId)
                    };
                    break;
                case MessageType.ExchangeInformationRequest:
                    // TODO
                    break;
                case MessageType.MoveRequest:
                    MoveRequest moveRequest = (MoveRequest)request.GetPayload();
                    MoveResponse moveResponse = map.Move(moveRequest.Direction, agentId);
                    responseMessage = new Message<MoveResponse>()
                    {
                        AgentId = agentId,
                        MessagePayload = moveResponse
                    };
                    break;
                case MessageType.PickPieceRequest:
                    break;
                case MessageType.PutPieceRequest:
                    break;
                case MessageType.RedirectedExchangeInformationRequest:
                    break;
                default:
                    break;
            }

            return responseMessage;
        }
    }
}
