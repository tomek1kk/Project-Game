using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class JoinGameResponse : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("accepted")]
        public bool? Accepted { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("agentID")]
        public int? AgentID { get; set; }

        public override bool ValidateMessage()
        {
            if (Accepted == null)
                return false;
            return true;
        }
    }
}
