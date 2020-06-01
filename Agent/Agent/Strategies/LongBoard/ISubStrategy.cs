namespace Agent.Strategies.LongBoard
{
    public interface ISubStrategy : IStrategy
    {
        bool IsDone(AgentInfo agentInfo);
    }
}