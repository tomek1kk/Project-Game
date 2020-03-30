using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary.Model;

namespace CommunicationLibrary.Response
{
    public class PutPieceResponse : MessagePayload
    {
        public PutResultEnum PutResult { get; set; }
        public override bool ValidateMessage()
        {
            return true;
        }
    }
}