using System;
using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Request;

namespace Agent
{
    public interface IStrategy
    {
        Field[,] Board {get; set;}
        Message MakeDecision(AgentInfo agent);
        void UpdateMap(Message message);

    }
    public class SampleStrategy : IStrategy
    {
        public Field[,] Board { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Message MakeDecision(AgentInfo agent)
        {
            return new Message<DiscoveryRequest>(new DiscoveryRequest());
        }

        void IStrategy.UpdateMap(Message message)
        {

        }
    }
}

