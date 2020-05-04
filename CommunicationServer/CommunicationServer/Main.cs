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
            Configuration config = Configuration.ReadConfiguration(args);
            CreateLogger(config.LoggingMode);
            Log.Information("Start communication server.");
            using (CommunicationServer communicationServer = new CommunicationServer(config))
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
        static void CreateLogger(string mode)
        {
            if(mode == "debug")
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("logs\\logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
        }
    }
}
