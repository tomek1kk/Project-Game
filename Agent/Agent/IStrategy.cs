using System;
using Agent.AgentBoard;
using CommunicationLibrary;

namespace Agent
{
    public interface IStrategy
    {
        Field[,] Board {get; set;}
        void MakeDecision(AgentInfo agent);
        void UpdateMap(Message message);

    }
    public class SampleStrategy : IStrategy
    {
        public Field[,] Board { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void MakeDecision(AgentInfo agent)
        {

        }

        void IStrategy.UpdateMap(Message message)
        {
            throw new NotImplementedException();
        }
    }
}

