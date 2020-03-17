using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public abstract class Message<T> : Message where T : MessagePayload
    {
        public T MessagePayload { get; }

        

    }
}
