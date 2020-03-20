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
        public void HandleMessage(Message message, IMessageSenderReceiver communicator)
        {
            if (message.GetPayload().ValidateMessage() == false)
            {
                communicator.Send(new Message<NotDefinedError>()
                {
                    AgentId = message.AgentId,
                    MessagePayload = new NotDefinedError()
                });
            }
            Message response;

            switch (message.MessageId)
            {
                case MessageType.JoinGameRequest:
                    response = new Message<JoinGameResponse>()
                    {
                        AgentId = message.AgentId,
                        MessagePayload = 
                    };
                    break;
                case MessageType.CheckHoldedPieceRequest:
                    break;
                case MessageType.DestroyPieceRequest:
                    break;
                case MessageType.DiscoveryRequest:
                    break;
                case MessageType.ExchangeInformationRequest:
                    break;
                case MessageType.MoveRequest:
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
        }
    }
}
