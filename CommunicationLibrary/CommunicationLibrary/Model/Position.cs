using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Model
{
    public class Position
    {
        [System.Text.Json.Serialization.JsonPropertyName("x")]
        public int? X { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("y")]
        public int? Y { get; set; }
    }
}
