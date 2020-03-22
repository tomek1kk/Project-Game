using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationLibrary;
using GameMaster.Game;
using GameMaster.MessageHandlers;

namespace GameMaster
{
    public class ProxyMessageHandler : IMessageHandler
    {
        private readonly Dictionary<MessageType, MessageHandler> handlers;

        private MoveRequestHandler moveRequestHandler = new MoveRequestHandler();
        private CheckHoldedPieceRequestHandler checkHoldedPieceRequestHandler = new CheckHoldedPieceRequestHandler();
        private DestroyPieceRequestHandler destroyPieceRequestHandler = new DestroyPieceRequestHandler();
        private DiscoveryRequestHandler discoveryRequestHandler = new DiscoveryRequestHandler();
        private ExchangeInformationRequestHandler exchangeInformationRequestHandler = new ExchangeInformationRequestHandler();
        private JoinGameRequestHandler joinGameRequestHandler = new JoinGameRequestHandler();
        private PickPieceRequestHandler pickPieceRequestHandler = new PickPieceRequestHandler();
        private PutPieceRequestHandler putPieceRequestHandler = new PutPieceRequestHandler();
        private RedirectExchangeInformationRequestHandler redirectExchangeInformationRequestHandler = new RedirectExchangeInformationRequestHandler();

        private MessageType messageId;

        public ProxyMessageHandler()
        {
            handlers = new Dictionary<MessageType, MessageHandler>()
            {
                { MessageType.MoveRequest, moveRequestHandler },
                { MessageType.CheckHoldedPieceRequest, checkHoldedPieceRequestHandler },
                { MessageType.DestroyPieceRequest, destroyPieceRequestHandler },
                { MessageType.DiscoveryRequest, discoveryRequestHandler },
                { MessageType.ExchangeInformationRequest, exchangeInformationRequestHandler },
                { MessageType.JoinGameRequest, joinGameRequestHandler },
                { MessageType.PickPieceRequest, pickPieceRequestHandler },
                { MessageType.PutPieceRequest, putPieceRequestHandler },
                { MessageType.RedirectedExchangeInformationRequest, redirectExchangeInformationRequestHandler }
            };
        }

        public void BaseReadMessage(Message message)
        {
            messageId = message.MessageId;
            handlers[messageId].BaseReadMessage(message);
        }

        public Message ProcessRequest(Map map)
        {
            return handlers[messageId].ProcessRequest(map);
        }

        public void SetTimeout()
        {
            handlers[messageId].SetTimeout();
        }
    }
}
