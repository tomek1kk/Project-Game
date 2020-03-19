using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class PickPieceResponse : MessagePayload
    {
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
