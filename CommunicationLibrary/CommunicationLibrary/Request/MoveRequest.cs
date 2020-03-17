﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Request
{
    public class MoveRequest : MessagePayload
    {
        public string Direction { get; set; }

        public override bool ValidateMessage()
        {
            if (Direction == "N" || Direction == "S" || Direction == "W" || Direction == "E")
                return true;
            return false;
        }
    }
}