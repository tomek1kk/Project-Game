using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Model
{
    public class NumberOfPlayers
    {
        [System.Text.Json.Serialization.JsonPropertyName("allies")]
        public int? Allies { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("enemies")]
        public int? Enemies { get; set; }
    }
}
