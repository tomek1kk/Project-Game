using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class JoinGameRequest : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("teamID")]
        public string TeamId { get; set; }

        public override bool ValidateMessage()
        {
            if (TeamId == "red" || TeamId == "blue")
                return true;
            return false;
        }
    }
}
