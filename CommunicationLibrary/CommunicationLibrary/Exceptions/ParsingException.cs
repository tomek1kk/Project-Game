using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Exceptions
{
    public class ParsingException : Exception
    {

        public ParsingException() : base("Parsing failed")
        {
        }
        public ParsingException(Exception e) : base("Parsing failed", e)
        {
        }
    }
}
