using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class CheckHoldedPieceResponse : IMessage
    {
        public bool? Sham { get; set; } 

        public bool ValidateMessage()
        {
            if (Sham == null)
                return false;
            return true;
        }
    }
}
