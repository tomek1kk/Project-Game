using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class PickPieceResponse : IMessage
    {
        public bool ValidateMessage()
        {
            return true;
        }
    }
}
