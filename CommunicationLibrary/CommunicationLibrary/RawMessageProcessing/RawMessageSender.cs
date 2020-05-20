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
            byte[] messageLengthBytes = BitConverter.GetBytes(messageBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(messageLengthBytes);
            byte[] allBytes = new byte[messageBytes.Length + messageLengthBytes.Length];
            for (int i = 0; i < messageLengthBytes.Length; i++)
                allBytes[i] = messageLengthBytes[i];
            for (int i = 0; i < messageBytes.Length; i++) 
                allBytes[i+messageLengthBytes.Length] = messageBytes[i];

            _byteStreamWriter(allBytes, allBytes.Length);
        }
    }
}
