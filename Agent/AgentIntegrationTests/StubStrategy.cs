using Agent;
using CommunicationLibrary;
using Agent.Strategies;
using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary.Request;

namespace AgentIntegrationTests
{
    class StubStrategy : Strategy
    {
        public override Message MakeDecision(AgentInfo agent)
        {
            return new Message<DiscoveryRequest>(new DiscoveryRequest());
        }
    }
}
