using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Model
{
    public class Penalties
    {
        [System.Text.Json.Serialization.JsonPropertyName("move")]
        public string Move { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("checkForSham")]
        public string CheckForSham { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("discovery")]
        public string Discovery { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("destroyPiece")]
        public string DestroyPiece { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("putPiece")]
        public string PutPiece { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("informationExchange")]
        public string InformationExchange { get; set; }
    }
}
