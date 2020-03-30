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
            CommunicationServer communicationServer = new CommunicationServer();

            Log.Information("Connect Game Master:");
            communicationServer.ConnectGameMaster();

            Log.Information("Connect Agents:");
            communicationServer.ConnectAgents();
            Console.WriteLine("Koniec CS");
        }
    }
}
