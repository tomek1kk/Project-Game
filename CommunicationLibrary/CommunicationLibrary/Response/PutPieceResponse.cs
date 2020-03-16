using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class PutPieceResponse : Message
    {
        public override bool ValidateMessage()
        {
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 110;
            }
        }
    }
}
