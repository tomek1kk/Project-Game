using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class DestroyPieceResponse : MessagePayload
    {
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
