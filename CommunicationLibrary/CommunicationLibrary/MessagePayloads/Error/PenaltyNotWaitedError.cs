using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class PenaltyNotWaitedError : MessagePayload
    {
        /// <summary>
        /// Wait time in miliseconds until agent can perform next action
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("waitFor")]
        public int WaitFor { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
