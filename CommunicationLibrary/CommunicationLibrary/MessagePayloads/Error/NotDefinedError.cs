using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class NotDefinedError : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("position")]
        public Position Position { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("holdingPiece")]
        public bool? HoldingPiece { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
