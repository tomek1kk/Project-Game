using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class PenaltyNotWaitedError : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("waitUntil")]
        public DateTime WaitUntill { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
