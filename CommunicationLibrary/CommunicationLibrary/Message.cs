using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public abstract class Message
    {
        public abstract bool ValidateMessage();
        public abstract int MessageId { get; }
        public int? AgentId { get; }

    }
}
