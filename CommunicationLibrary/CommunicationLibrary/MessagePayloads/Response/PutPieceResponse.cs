using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary.Model;

namespace CommunicationLibrary.Response
{
    public class PutPieceResponse : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("putResult")]
        public PutResultEnum PutResult { get; set; }
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}