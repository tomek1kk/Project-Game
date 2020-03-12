using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class NotDefinedError : IMessage
    {
        public Point Position { get; set; }
        public bool? HoldingPiece { get; set; }

        public bool ValidateMessage()
        {
            return true;
        }
    }
}
