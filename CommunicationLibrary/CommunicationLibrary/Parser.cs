using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommunicationLibrary
{
    public class Parser : IParser
    {
        JsonSerializerOptions options;
        public Parser()
        {
            options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
        }

        public string AsString(Message message)
        {
            return JsonSerializer.Serialize(message, message.GetType(), options);
        }

        class EmptyMessage
        {
            [System.Text.Json.Serialization.JsonPropertyName("messageID")]
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
