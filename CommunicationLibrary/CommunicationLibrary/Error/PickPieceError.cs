using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class PickPieceError : IMessage
    {
        public string ErrorSubtype { get; set; }

        public bool ValidateMessage()
        {
            if (ErrorSubtype == null || (ErrorSubtype != "NothingThere" && ErrorSubtype != "Other")
                return false;
            return true;
        }
    }
}
