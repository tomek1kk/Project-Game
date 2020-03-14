using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class NotDefinedError : Message
    {
        public (int? x, int? y) Position { get; set; }
        public bool? HoldingPiece { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
