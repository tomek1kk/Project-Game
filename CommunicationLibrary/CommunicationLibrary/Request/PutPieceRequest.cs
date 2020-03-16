using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class PutPieceRequest : Message
    {
        public override bool ValidateMessage()
        {
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 9;
            }
        }
    }
}
