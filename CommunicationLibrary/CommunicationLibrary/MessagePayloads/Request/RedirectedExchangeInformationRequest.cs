using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class RedirectedExchangeInformationRequest : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("askingID")]
        public int? AskingId { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("leader")]
        public bool? Leader { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("teamID")]
        public string TeamId { get; set; }

        public override bool ValidateMessage()
        {
            if (AskingId == null || Leader == null || TeamId == null)
                return false;
            return true;
        }
    }
}
