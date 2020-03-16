using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class DestroyPieceRequest : Message
    {
        public override bool ValidateMessage()
        {
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 2;
            }
        }
    }
}
