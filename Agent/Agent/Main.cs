using System;
using System.Collections.Generic;
using System.Text;

namespace Agent
{
    public class AgentProgram
    {
        static void Main(string [] args)
        {
            AgentConfiguration configuration = AgentConfiguration.ReadConfiguration(args);
            Agent agent = new Agent(configuration);
            agent.StartListening();
        }
    }
}
