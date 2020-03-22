using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class MoveRequestHandler : MessageHandler
    {
        private string direction;

        protected override void ReadMessage(MessagePayload payload)
        {
            MoveRequest request = (MoveRequest)payload;
            direction = request.Direction;
        }

        protected override bool CheckRequest(Map map)
        {
            return true; // TODO
        }

        protected override void Execute(Map map)
        {
            throw new NotImplementedException();
        }

        protected override Message GetResponse(Map map)
        {
            return new Message<MoveResponse>()
            {
                MessagePayload = new MoveResponse()
                {
                    MadeMove = false,
                    CurrentPosition = map.GetPosition(agentId),
                    ClosestPiece = map.GetClosestPiece(agentId)
                }
            };
        }
    }
}
