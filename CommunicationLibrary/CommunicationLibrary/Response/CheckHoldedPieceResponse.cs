using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class CheckHoldedPieceResponse : Message
    {
        public bool? Sham { get; set; }

        public override int MessageId
        {
            get
            {
                return 101;
            }
        }

        public override bool ValidateMessage()
        {
            if (Sham == null)
                return false;
            return true;
        }
    }
}
