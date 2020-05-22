using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class ExchangeInformationRequest : MessagePayload
    {
        public override bool ValidateMessage()
        {
            if (AskedAgentId == null)
                return false;
            return true;
        }

        [System.Text.Json.Serialization.JsonPropertyName("askedAgentID")]
        public int? AskedAgentId { get; set; }
    }
}
