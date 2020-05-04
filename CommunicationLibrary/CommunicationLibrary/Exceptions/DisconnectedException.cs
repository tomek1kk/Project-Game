using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Exceptions
{
    public class DisconnectedException : Exception
    {

        public DisconnectedException() : base("Other side disconnected")
        {
        }
        public DisconnectedException(Exception e):base("Other side disconnected", e)
        {
        }
    }
}
