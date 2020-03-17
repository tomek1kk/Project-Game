using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class PutPieceError : MessagePayload
    {
        public string ErrorSubtype { get; set; }

        public override bool ValidateMessage()
        {
            if (ErrorSubtype == null || (ErrorSubtype != "AgentNotHolding" && ErrorSubtype != "Other" && ErrorSubtype != "CannotPutThere"))
                return false;
            return true;
        }
    }
}
