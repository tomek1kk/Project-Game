using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class MoveError : IMessage
    {
        public Point Position { get; set; }

        public bool ValidateMessage()
        {
            if (Position == null || Position.X == null || Position.Y == null)
                return false;
            return true;
        }
    }
}
