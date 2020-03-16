using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class MoveResponse : Message
    {
        public bool? MadeMove { get; set; }
        public (int? x, int? y) CurrentPosition { get; set; }
        public int? ClosestPiece { get; set; }

        public override bool ValidateMessage()
        {
            if (MadeMove == null)
                return false;
            if (CurrentPosition.x == null || CurrentPosition.y == null)
                return false;
            if (ClosestPiece == null || ClosestPiece < 0)
                return false;
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 108;
            }
        }
    }
}
