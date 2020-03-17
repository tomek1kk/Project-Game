using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    public abstract class Message
    {
        public abstract MessageType MessageId
        {
            get;
            set;
        }
        public int? AgentId { get; }
        
    }
}
