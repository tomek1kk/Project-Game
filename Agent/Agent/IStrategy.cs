using System;
namespace Agent
{
    public interface IStrategy
    {
        void MakeDecision(AgentInfo agent);

    }
    public class SampleStrategy : IStrategy
    { 
        public void MakeDecision(AgentInfo agent)
        {

        }
    }
}

