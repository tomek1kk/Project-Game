using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class PutPieceResponse : IMessage
    {
        public bool ValidateMessage()
        {
            return true;
        }
    }
}
