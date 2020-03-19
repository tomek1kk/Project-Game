using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class PutPieceRequest : MessagePayload
    {
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
