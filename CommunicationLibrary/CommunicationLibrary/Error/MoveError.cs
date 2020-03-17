using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class MoveError : MessagePayload
    {
        public (int? x, int? y) Position { get; set; }

        public override bool ValidateMessage()
        {
            if (Position.x == null || Position.y == null)
                return false;
            return true;
        }
    }
}
