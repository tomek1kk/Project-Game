using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class MoveRequest : Message
    {
        public string Direction { get; set; }

        public override bool ValidateMessage()
        {
            if (Direction == "N" || Direction == "S" || Direction == "W" || Direction == "E")
                return true;
            return false;
        }
        public override int MessageId
        {
            get
            {
                return 7;
            }
        }
    }
}
