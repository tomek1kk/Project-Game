using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Strategies
{
    public class SampleStrategy : Strategy
    {
        public SampleStrategy(int width, int height) : base(width, height)
        {
        }

        public override Message MakeDecision(AgentInfo agent)
        {
            return new Message<DiscoveryRequest>(new DiscoveryRequest());
        }
    }
}
