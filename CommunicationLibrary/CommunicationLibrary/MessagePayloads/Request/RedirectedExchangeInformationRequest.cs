using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class RedirectedExchangeInformationRequest : MessagePayload
    {
        public int? AskingId { get; set; }
        public bool? Leader { get; set; }
        public string TeamId { get; set; }

        public override bool ValidateMessage()
        {
            if (AskingId == null || Leader == null || TeamId == null)
                return false;
            return true;
        }
    }
}
