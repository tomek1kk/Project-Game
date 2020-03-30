using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.RawMessageProcessing
{
    class RawMessageSender
    {
        public delegate void Write(byte[] buffer, int count);
        private Write _byteStreamWriter;
        public RawMessageSender(Write byteStreamWriter)
        {
            _byteStreamWriter = byteStreamWriter;
        }
        public void SendMessage(string messageString)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
            byte[] messageLengthBytes = BitConverter.GetBytes((ushort)messageBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(messageLengthBytes);

            _byteStreamWriter(messageLengthBytes, messageLengthBytes.Length);
            _byteStreamWriter(messageBytes, messageBytes.Length);
        }
    }
}
