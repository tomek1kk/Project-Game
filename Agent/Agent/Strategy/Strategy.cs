using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Agent.Strategies
{
    abstract public class Strategy : IStrategy
    {
        public Field[,] Board { get; private set; }

        public Strategy(int width, int height)
        {
            Board = new Field[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Board[i, j] = new Field();
        }

        virtual public Message MakeDecision(AgentInfo agent)
        {
            throw new Exception("Not implemented strategy!");
        }

        public void UpdateMap(Message message, Point position)
        {
            switch (message.MessageId)
            {
                case MessageType.CheckHoldedPieceResponse:
                    CheckHoldedPieceResponseHandler((CheckHoldedPieceResponse)message.GetPayload());
                    break;
                case MessageType.DiscoveryResponse:
                    DiscoveryResponseHandler((DiscoveryResponse)message.GetPayload(), position);
                    break;
                case MessageType.ExchangeInformationResponse:
                    ExchangeInformationResponseHandler((ExchangeInformationResponse)message.GetPayload());
                    break;
                case MessageType.DestroyPieceResponse:
                    DestroyPieceResponseHandler((DestroyPieceResponse)message.GetPayload());
                    break;
                case MessageType.MoveResponse:
                    MoveResponseHandler((MoveResponse)message.GetPayload());
                    break;
                case MessageType.PickPieceResponse:
                    PickPieceResponseHandler((PickPieceResponse)message.GetPayload());
                    break;
                case MessageType.PutPieceResponse:
                    PutPieceResponseHandler((PutPieceResponse)message.GetPayload());
                    break;
                case MessageType.MoveError:
                    MoveErrorResponseHandler((MoveError)message.GetPayload());
                    break;
                case MessageType.PickPieceError:
                    PickPieceErrorResponseHandler((PickPieceError)message.GetPayload());
                    break;
                case MessageType.PutPieceError:
                    PutPieceErrorResponseHandler((PutPieceError)message.GetPayload());
                    break;
                case MessageType.PenaltyNotWaitedError:
                    PenaltyNotWaitedErrorResponseHandler((PenaltyNotWaitedError)message.GetPayload());
                    break;
                case MessageType.NotDefinedError:
                    NotDefinedResponseHandler((NotDefinedError)message.GetPayload());
                    break;
            }
        }

        virtual protected void CheckHoldedPieceResponseHandler(CheckHoldedPieceResponse checkHoldedPieceResponse) { }
        virtual protected void DiscoveryResponseHandler(DiscoveryResponse discoveryResponse, Point positon)
        {
            if (discoveryResponse.DistanceNW.HasValue) Board[positon.X - 1, positon.Y + 1].DistToPiece = discoveryResponse.DistanceNW.Value;
            if (discoveryResponse.DistanceN.HasValue) Board[positon.X, positon.Y + 1].DistToPiece = discoveryResponse.DistanceN.Value;
            if (discoveryResponse.DistanceNE.HasValue) Board[positon.X + 1, positon.Y + 1].DistToPiece = discoveryResponse.DistanceNE.Value;
            if (discoveryResponse.DistanceW.HasValue) Board[positon.X - 1, positon.Y].DistToPiece = discoveryResponse.DistanceW.Value;
            if (discoveryResponse.DistanceFromCurrent.HasValue) Board[positon.X, positon.Y].DistToPiece = discoveryResponse.DistanceFromCurrent.Value;
            if (discoveryResponse.DistanceE.HasValue) Board[positon.X + 1, positon.Y].DistToPiece = discoveryResponse.DistanceE.Value;
            if (discoveryResponse.DistanceSW.HasValue) Board[positon.X - 1, positon.Y - 1].DistToPiece = discoveryResponse.DistanceSW.Value;
            if (discoveryResponse.DistanceS.HasValue) Board[positon.X, positon.Y - 1].DistToPiece = discoveryResponse.DistanceS.Value;
            if (discoveryResponse.DistanceSE.HasValue) Board[positon.X + 1, positon.Y - 1].DistToPiece = discoveryResponse.DistanceSE.Value;
        }
        virtual protected void DestroyPieceResponseHandler(DestroyPieceResponse moveError) { }
        virtual protected void ExchangeInformationResponseHandler(ExchangeInformationResponse exchangeInformationResponse)
        {
            //dont know how to interpret Enumerable<int> in response.
        }
        virtual protected void MoveResponseHandler(MoveResponse moveResponse)
        {
            Board[moveResponse.CurrentPosition.X.Value, moveResponse.CurrentPosition.Y.Value].DistToPiece = moveResponse.ClosestPiece.Value;
        }
        virtual protected void NotDefinedResponseHandler(NotDefinedError notDefinedError) { }
        virtual protected void MoveErrorResponseHandler(MoveError moveError) { }
        virtual protected void PickPieceErrorResponseHandler(PickPieceError pieceError) { }
        virtual protected void PickPieceResponseHandler(PickPieceResponse pickPieceRespone) { }
        virtual protected void PutPieceErrorResponseHandler(PutPieceError putPieceError) { }
        virtual protected void PutPieceResponseHandler(PutPieceResponse pickPieceRespone) { }
        virtual protected void PenaltyNotWaitedErrorResponseHandler(PenaltyNotWaitedError pickPieceRespone) { }


    }
}
