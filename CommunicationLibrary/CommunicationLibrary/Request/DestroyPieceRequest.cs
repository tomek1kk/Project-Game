using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class DestroyPieceRequest : IMessage
    {
        public bool ValidateMessage()
        {
            return true;
        }
    }
}
