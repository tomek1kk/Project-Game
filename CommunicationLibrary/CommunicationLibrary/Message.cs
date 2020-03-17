using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public class Message<T> : Message where T : MessagePayload
    {
        public Message(T payload)
        {
            MessagePayload = payload;
        }
        //for json parser
        public Message() { }
        public T MessagePayload { get; set; }
        public override MessageType MessageId { get => messageDictionary[typeof(T)]; set { } }

        private static Dictionary<Type, MessageType> messageDictionary = new Dictionary<Type, MessageType>()
        {
            { typeof(CheckHoldedPieceRequest), MessageType.CheckHoldedPieceRequest },
            { typeof(DestroyPieceRequest), MessageType.DestroyPieceRequest },
            { typeof(DiscoveryRequest), MessageType.DiscoveryRequest },
            { typeof(ExchangeInformationRequest), MessageType.ExchangeInformationRequest },
            { typeof(JoinGameRequest), MessageType.JoinGameRequest },
            { typeof(MoveRequest), MessageType.MoveRequest },
            { typeof(PickPieceRequest), MessageType.PickPieceRequest },
            { typeof(PutPieceRequest), MessageType.PutPieceRequest },
            { typeof(CheckHoldedPieceResponse), MessageType.CheckHoldedPieceResponse },
            { typeof(DestroyPieceResponse), MessageType.DestroyPieceResponse },
            { typeof(DiscoveryResponse), MessageType.DiscoveryResponse },
            { typeof(ExchangeInformationResponse), MessageType.ExchangeInformationResponse },
            { typeof(JoinGameResponse), MessageType.JoinGameResponse },
            { typeof(MoveResponse), MessageType.MoveResponse },
            { typeof(PickPieceResponse), MessageType.PickPieceResponse },
            { typeof(PutPieceResponse), MessageType.PutPieceResponse },
            { typeof(MoveError), MessageType.MoveError },
            { typeof(NotDefinedError), MessageType.NotDefinedError },
            { typeof(PickPieceError), MessageType.PickPieceError },
            { typeof(PutPieceError), MessageType.PutPieceError },
        };

    }
}
