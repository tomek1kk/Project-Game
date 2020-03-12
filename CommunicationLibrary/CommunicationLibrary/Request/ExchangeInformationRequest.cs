using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class ExchangeInformationRequest : IMessage
    {
        public bool ValidateMessage()
        {
            if (AskedAgentId == null)
                return false;
            return true;
        }

        public int? AskedAgentId { get; set; }
    }
}
