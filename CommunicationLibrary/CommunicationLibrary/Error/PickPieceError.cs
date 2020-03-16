using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class PickPieceError : Message
    {
        public string ErrorSubtype { get; set; }

        public override bool ValidateMessage()
        {
            if (ErrorSubtype == null || (ErrorSubtype != "NothingThere" && ErrorSubtype != "Other"))
                return false;
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 902;
            }
        }
    }
}
