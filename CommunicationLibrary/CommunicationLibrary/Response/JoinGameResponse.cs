using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class JoinGameResponse : Message
    {
        public bool? Accepted { get; set; }
        public int? AgentID { get; set; }

        public override bool ValidateMessage()
        {
            if (Accepted == null)
                return false;
            return true;
        }
    }
}
