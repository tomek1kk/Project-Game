using CommunicationLibrary;
using GameMaster.Game;
using GameMaster.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    public static class RequestHandlerProvider
    {
        private static readonly Dictionary<MessageType, MessageHandler> handlers = new Dictionary<MessageType, MessageHandler>()
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
        private static MoveRequestHandler moveRequestHandler = new MoveRequestHandler();
        private static CheckHoldedPieceRequestHandler checkHoldedPieceRequestHandler = new CheckHoldedPieceRequestHandler();
        private static DestroyPieceRequestHandler destroyPieceRequestHandler = new DestroyPieceRequestHandler();
        private static DiscoveryRequestHandler discoveryRequestHandler = new DiscoveryRequestHandler();
        private static ExchangeInformationRequestHandler exchangeInformationRequestHandler = new ExchangeInformationRequestHandler();
        private static JoinGameRequestHandler joinGameRequestHandler = new JoinGameRequestHandler();
        private static PickPieceRequestHandler pickPieceRequestHandler = new PickPieceRequestHandler();
        private static PutPieceRequestHandler putPieceRequestHandler = new PutPieceRequestHandler();
        private static RedirectExchangeInformationRequestHandler redirectExchangeInformationRequestHandler = new RedirectExchangeInformationRequestHandler();

        public static MessageHandler GetHandler(MessageType messageId)
        {
            return handlers[messageId];
        }

    }
}
