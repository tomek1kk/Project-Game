using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public class Parser : IParser
    {

        public string AsString<T>(T message) where T : MessagePayload
        {
            return JsonSerializer.Serialize<T>(message);
        }

        class EmptyMessage
        {
            public int MessageId { get; set; }
        }
        public Message Parse(string messageString) where T : MessagePayload
        {
            EmptyMessage message = (EmptyMessage)JsonSerializer.Deserialize(messageString, typeof(EmptyMessage));

            switch (message.MessageId)
            {
                #region Requests
                case 1:
                    return (Message<T>)JsonSerializer.Deserialize(messageString, typeof(Message<T>));
                case 2:
                    return (DestroyPieceRequest)JsonSerializer.Deserialize(messageString, typeof(DestroyPieceRequest));
                case 3:
                    return (DiscoveryRequest)JsonSerializer.Deserialize(messageString, typeof(DiscoveryRequest));
                case 5:
                    return (ExchangeInformationRequest)JsonSerializer.Deserialize(messageString, typeof(ExchangeInformationRequest));
                case 6:
                    return (JoinGameRequest)JsonSerializer.Deserialize(messageString, typeof(JoinGameRequest));
                case 7:
                    return (MoveRequest)JsonSerializer.Deserialize(messageString, typeof(MoveRequest));
                case 8:
                    return (PickPieceRequest)JsonSerializer.Deserialize(messageString, typeof(PickPieceRequest));
                case 9:
                    return (PutPieceRequest)JsonSerializer.Deserialize(messageString, typeof(PutPieceRequest));
                #endregion
                #region Responses
                case 101:
                    return (CheckHoldedPieceResponse)JsonSerializer.Deserialize(messageString, typeof(CheckHoldedPieceResponse));
                case 102:
                    return (DestroyPieceResponse)JsonSerializer.Deserialize(messageString, typeof(DestroyPieceResponse));
                case 103:
                    return (DiscoveryResponse)JsonSerializer.Deserialize(messageString, typeof(DiscoveryResponse));
                case 4:
                    return (ExchangeInformationResponse)JsonSerializer.Deserialize(messageString, typeof(ExchangeInformationResponse));
                case 107:
                    return (JoinGameResponse)JsonSerializer.Deserialize(messageString, typeof(JoinGameResponse));
                case 108:
                    return (MoveResponse)JsonSerializer.Deserialize(messageString, typeof(MoveResponse));
                case 109:
                    return (PickPieceResponse)JsonSerializer.Deserialize(messageString, typeof(PickPieceResponse));
                case 110:
                    return (PutPieceResponse)JsonSerializer.Deserialize(messageString, typeof(PutPieceResponse));
                #endregion
                #region Errors
                case 901:
                    return (Message)JsonSerializer.Deserialize(messageString, typeof(Message<MoveError>));
                case 902:
                    return (PickPieceError)JsonSerializer.Deserialize(messageString, typeof(PickPieceError));
                case 903:
                    return (PutPieceError)JsonSerializer.Deserialize(messageString, typeof(PutPieceError));
                case 905:
                    return (NotDefinedError)JsonSerializer.Deserialize(messageString, typeof(NotDefinedError));
                #endregion
                default:
                    return new NotDefinedError();

            }
            
        }
    }
}
