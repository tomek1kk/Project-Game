using System;
using System.Collections.Generic;
using System.Text;

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
