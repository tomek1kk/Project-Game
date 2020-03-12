using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class MoveRequest : IMessage
    {
        public string Direction { get; set; }

        public bool ValidateMessage()
        {
            if (Direction == "N" || Direction == "S" || Direction == "W" || Direction == "E")
                return true;
            return false;
        }
    }
}
