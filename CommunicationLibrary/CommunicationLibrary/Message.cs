using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public abstract class Message
    {
        public abstract bool ValidateMessage();

        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
