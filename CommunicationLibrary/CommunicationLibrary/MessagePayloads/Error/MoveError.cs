using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class MoveError : MessagePayload
    {
        public Position Position { get; set; }

        public override bool ValidateMessage()
        {
            if (Position.x == null || Position.y == null)
                return false;
            return true;
        }
    }
}
