using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Exceptions
{
    public class AgentInfoNotValidException : Exception
    {
        public AgentInfoNotValidException() : base("AgentInfoNotValid") { }
        public AgentInfoNotValidException(string message)
            : base("Agent info not valid:" + message) { }
        public AgentInfoNotValidException(string message, Exception inner)
            : base("Agent info not valid:" + message, inner) { }
    }
}
