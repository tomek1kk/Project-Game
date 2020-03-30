using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerNamespace.Helpers
{
    public static class MessageExtension
    {
        public static bool IsEndGame(this Message message)
        {
            return message.MessageId == MessageType.GameEnded;
        }
        public static bool IsGameStarted(this Message message)
        {
            return message.MessageId == MessageType.GameStarted;
        }
    }
}
