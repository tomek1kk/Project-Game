using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Information
{
    public class GameEnded : MessagePayload
    {
        public string Winner { get; set; }

        public override bool ValidateMessage()
        {
            if (Winner == "red" || Winner == "blue")
                return true;
            return false;
        }
    }
}
