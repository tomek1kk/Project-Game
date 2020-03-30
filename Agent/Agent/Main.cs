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
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("Logs\\AgentLog-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            Log.Information("Agent started");
            AgentConfiguration configuration = AgentConfiguration.ReadConfiguration(args);
            using (Agent agent = new Agent(configuration))
                agent.StartListening();
        }
    }
}
