using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public static class JsonParser
    {
        public static string ToJSON<T>(T message) where T : Message
        {
            return JsonSerializer.Serialize<T>(message);
        }
    }
}
