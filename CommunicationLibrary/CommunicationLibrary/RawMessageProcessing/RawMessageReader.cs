using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.RawMessageProcessing
{
    public class RawMessageReader
    {
        public delegate int Read(byte[] buffer, int count);
        private readonly Read _byteStreamReader;
        private readonly byte[] _messageBuffer;
        private readonly byte[] _messageLengthBuffer;

        public RawMessageReader(Read byteStreamReader)
        {
            _byteStreamReader = byteStreamReader;
            _messageBuffer = new byte[8 * 1024];
            _messageLengthBuffer = new byte[2];
        }
        public string GetNextMessageAsString()
        {
            int bytesRead = _byteStreamReader(_messageLengthBuffer, 4);
            if (bytesRead != 4) throw new Exception("Failed to read bytes");
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(_messageLengthBuffer);
            int messageLength = BitConverter.ToInt32(_messageLengthBuffer, 0);
            if (messageLength == 0) return String.Empty;
            bytesRead = _byteStreamReader(_messageBuffer, messageLength);
            if (bytesRead == 0) throw new Exception("Failed to read bytes");
            return Encoding.UTF8.GetString(_messageBuffer, 0, bytesRead);
        }
    }
}
