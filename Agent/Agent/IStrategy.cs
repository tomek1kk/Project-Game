using System;
using Agent.AgentBoard;

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
        public void MakeDecision(AgentInfo agent)
        {

        }
        void UpdateMap(Message message)
        {

        }
    }
}

