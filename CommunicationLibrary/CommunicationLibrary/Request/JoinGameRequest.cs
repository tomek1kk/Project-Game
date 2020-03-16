using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class JoinGameRequest : Message
    {
        public string TeamId { get; set; }

        public override bool ValidateMessage()
        {
            if (TeamId != "red" || TeamId != "blue")
                return false;
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 6;
            }
        }
    }
}
