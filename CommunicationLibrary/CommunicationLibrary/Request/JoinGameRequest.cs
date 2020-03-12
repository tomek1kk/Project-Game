using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class JoinGameRequest : IMessage
    {
        public string TeamId { get; set; }

        public bool ValidateMessage()
        {
            if (TeamId != "red" || TeamId != "blue")
                return false;
            return true;
        }
    }
}
