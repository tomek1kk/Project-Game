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
        public abstract int MessageId
        {
            get;
        }
        public int? AgentId { get; }
        
    }
}
