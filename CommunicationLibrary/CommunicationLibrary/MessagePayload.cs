using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    public abstract class MessagePayload
    {
        public abstract bool ValidateMessage();
    }
}
