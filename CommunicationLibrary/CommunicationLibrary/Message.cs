using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public abstract class Message<T> : Message where T : MessagePayload
    {
        public T MessagePayload { get; }
        public override int MessageId => messageDictionary[typeof(T)];
        private static Dictionary<Type, int> messageDictionary = new Dictionary<Type, int>()
        {
            { typeof(CheckHoldedPieceRequest), (int)MessageType.CheckHoldedPieceRequest },
            { typeof(DestroyPieceRequest), (int)MessageType.DestroyPieceRequest },
            { typeof(DiscoveryRequest), (int)MessageType.DiscoveryRequest },
            { typeof(ExchangeInformationRequest), (int)MessageType.ExchangeInformationRequest },
            { typeof(JoinGameRequest), (int)MessageType.JoinGameRequest },
            { typeof(MoveRequest), (int)MessageType.MoveRequest },
            { typeof(PickPieceRequest), (int)MessageType.PickPieceRequest },
            { typeof(PutPieceRequest), (int)MessageType.PutPieceRequest },
            { typeof(CheckHoldedPieceResponse), (int)MessageType.CheckHoldedPieceResponse },
            { typeof(DestroyPieceResponse), (int)MessageType.DestroyPieceResponse },
            { typeof(DiscoveryResponse), (int)MessageType.DiscoveryResponse },
            { typeof(ExchangeInformationResponse), (int)MessageType.ExchangeInformationResponse },
            { typeof(JoinGameResponse), (int)MessageType.JoinGameResponse },
            { typeof(MoveResponse), (int)MessageType.MoveResponse },
            { typeof(PickPieceResponse), (int)MessageType.PickPieceResponse },
            { typeof(PutPieceResponse), (int)MessageType.PutPieceResponse },
            { typeof(MoveError), (int)MessageType.MoveError },
            { typeof(NotDefinedError), (int)MessageType.NotDefinedError },
            { typeof(PickPieceError), (int)MessageType.PickPieceError },
            { typeof(PutPieceError), (int)MessageType.PutPieceError },
        };

    }
}
