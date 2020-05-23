using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Error
{
    public class MoveError : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("position")]
        public Position Position { get; set; }

        public override bool ValidateMessage()
        {
            if (Position.X == null || Position.Y == null)
                return false;
            return true;
        }
    }
}
