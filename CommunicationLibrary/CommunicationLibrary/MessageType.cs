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
}
