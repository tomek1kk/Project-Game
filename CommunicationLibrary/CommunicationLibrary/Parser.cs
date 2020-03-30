using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public class Parser : IParser
    {

        public string AsString(Message message)
        {
            return JsonSerializer.Serialize(message, message.GetType());
        }

        class EmptyMessage
        {
            public MessageType MessageId { get; set; }
        }
        public Message Parse(string messageString)
        {
            EmptyMessage message = (EmptyMessage)JsonSerializer.Deserialize(messageString, typeof(EmptyMessage));
            return (Message)JsonSerializer.Deserialize(messageString,
                typeof(Message<>).MakeGenericType(message.MessageId.GetObjectType()));
            
            
        }
    }
}
