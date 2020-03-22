using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationLibrary;
using GameMaster.Configuration;
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

        public Message ProcessRequest(Map map, Message message, GMConfiguration configuration)
        {
            return handlers[message.MessageId].ProcessRequest(map, message, configuration);
        }
    }
}
