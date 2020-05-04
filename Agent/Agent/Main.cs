using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Agent
{
    public class AgentProgram
    {
        static void Main(string[] args)
        {

            AgentConfiguration configuration = AgentConfiguration.ReadConfiguration(args);
            CreateLogger(configuration.LoggingMode);
            using (Agent agent = new Agent(configuration))
                agent.StartListening();
        }

        static void CreateLogger(string mode)
        {
            if (mode == "debug")
            {
                Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("Logs\\AgentLog-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs\\AgentLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
        }
    }
}
