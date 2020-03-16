using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class ExchangeInformationRequest : Message
    {
        public override bool ValidateMessage()
        {
            if (AskedAgentId == null)
                return false;
            return true;
        }
        public override int MessageId
        {
            get
            {
                return 5;
            }
        }

        public int? AskedAgentId { get; set; }
    }
}
