using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class JoinGameResponse : IMessage
    {
        public bool? Accepted { get; set; }
        public int? AgentID { get; set; }

        public bool ValidateMessage()
        {
            if (Accepted == null)
                return false;
            return true;
        }
    }
}
