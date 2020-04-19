using System;
using Serilog;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerNamespace
{
    class MainCS
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information("Start communication server.");
            using (CommunicationServer communicationServer = new CommunicationServer())
            {
                Log.Information("Start connecting Game Master:");
                communicationServer.StartConnectingGameMaster();

                Log.Information("Accepting Game Master:");
                communicationServer.AcceptGameMaster();

                Log.Information("Connect Agents:");
                communicationServer.ConnectAgents();

                Log.Information("Wait for game over");
                communicationServer.WaitForGameOver();

                Console.WriteLine("Koniec CS");
            }

        }
    }
}
