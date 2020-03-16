using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class DestroyPieceResponse : Message
    {
        public override bool ValidateMessage()
        {
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 102;
            }
        }
    }
}
