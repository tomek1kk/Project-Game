using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Exceptions
{
    public class ParsingException : Exception
    {
        public string IncorrectMessage { get; }
        public ParsingException(string incorrectMessage) : base("Parsing failed")
        {
            IncorrectMessage = incorrectMessage;
        }
        public ParsingException(string incorrectMessage, Exception e) : base("Parsing failed", e)
        {
            IncorrectMessage = incorrectMessage;
        }
    }
}
