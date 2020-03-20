using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServer
{
    class MainCS
    {
        static void Main(string[] args)
        {
            CommunicationServer communicationServer = new CommunicationServer();
            communicationServer.ConnectGameMaster();
            communicationServer.ConnectAgents();
            Console.WriteLine("Koniec CS");
        }
    }
}
