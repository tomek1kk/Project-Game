using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class PickPieceResponse : Message
    {
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
