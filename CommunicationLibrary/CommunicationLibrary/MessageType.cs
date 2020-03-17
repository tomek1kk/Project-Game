using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    public enum MessageType
    {
        CheckHoldedPieceRequest = 1,
        DestroyPieceRequest = 2,
        DiscoveryRequest = 3,
        ExchangeInformationRequest = 5,
        JoinGameRequest = 6,
        MoveRequest = 7,
        PickPieceRequest = 8,
        PutPieceRequest = 9,
        CheckHoldedPieceResponse = 101,
        DestroyPieceResponse = 102,
        DiscoveryResponse = 103,
        ExchangeInformationResponse = 4,
        JoinGameResponse = 107,
        MoveResponse = 108,
        PickPieceResponse = 109,
        PutPieceResponse = 110,
        MoveError = 901,
        NotDefinedError = 905,
        PickPieceError = 902,
        PutPieceError = 903
    }
    public static class MessageTypeExtensions
    {
        public static Type GetType(this MessageType type) => messageTypeDictionary[type];

    private static Dictionary<MessageType, Type> messageTypeDictionary = new Dictionary<MessageType, Type>()
        {
            { MessageType.CheckHoldedPieceRequest, typeof(CheckHoldedPieceRequest) },
            { MessageType.DestroyPieceRequest, typeof(DestroyPieceRequest) },
            { MessageType.DiscoveryRequest, typeof(DiscoveryRequest) },
            { MessageType.ExchangeInformationRequest, typeof(ExchangeInformationRequest) },
            { MessageType.JoinGameRequest, typeof(JoinGameRequest) },
            { MessageType.MoveRequest, typeof(MoveRequest) },
            { MessageType.PickPieceRequest, typeof(PickPieceRequest) },
            { MessageType.PutPieceRequest, typeof(PutPieceRequest) },
            { MessageType.CheckHoldedPieceResponse, typeof(CheckHoldedPieceResponse) },
            { MessageType.DestroyPieceResponse, typeof(DestroyPieceResponse) },
            { MessageType.DiscoveryResponse, typeof(DiscoveryResponse) },
            { MessageType.ExchangeInformationResponse, typeof(ExchangeInformationResponse) },
            { MessageType.JoinGameResponse, typeof(JoinGameResponse) },
            { MessageType.MoveResponse, typeof(MoveResponse) },
            { MessageType.PickPieceResponse, typeof(PickPieceResponse) },
            { MessageType.PutPieceResponse, typeof(PutPieceResponse) },
            { MessageType.MoveError, typeof(MoveError) },
            { MessageType.NotDefinedError, typeof(NotDefinedError) },
            { MessageType.PickPieceError, typeof(PickPieceError) },
            { MessageType.PutPieceError, typeof(PutPieceError) }
        };
    }
}
