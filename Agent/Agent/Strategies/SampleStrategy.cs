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
        public SampleStrategy(int width, int height) : base(width, height) {}

        public override Message MakeDecision(AgentInfo agent)
        {
            Message m;
            //sprawdzic requesty o zapytanie informacji

            var last = History.Pop();
            History.Push(last);
            if (!agent.HasPiece && agent.inGoalArea())
            {
                var req = new MoveRequest();
                req.Direction = agent.GoalDirection == "N" ? "S" : "N";
                m = new Message<MoveRequest>(req);
            }
            else if (last == MessageType.DiscoveryRequest)
            {
                var req = new MoveRequest();

                int N = Board[agent.Position.X, agent.Position.Y + 1].DistToPiece;
                int S = Board[agent.Position.X, agent.Position.Y - 1].DistToPiece;
                int W = Board[agent.Position.X + 1, agent.Position.Y].DistToPiece;
                int E = Board[agent.Position.X - 1, agent.Position.Y].DistToPiece;
                int min = Math.Min(Math.Min(Math.Min(S, N), E), W);
                if (min == N)
                    req.Direction = "N";
                else if(min == S)
                    req.Direction = "S";
                else if(min == W)
                    req.Direction = "W";
                else if(min == E)
                    req.Direction = "W";                
                m = new Message<MoveRequest>(req);
            }
            else if (Board[agent.Position.X, agent.Position.Y].DistToPiece == 0)
            {
                m = new Message<PickPieceRequest>(new PickPieceRequest());
            }
            else if (agent.HasPiece && !agent.inGoalArea())
            {
                var req = new MoveRequest();
                req.Direction = agent.GoalDirection;
                m = new Message<MoveRequest>(req);
            }
            else
                m = new Message<DiscoveryRequest>(new DiscoveryRequest());
            

            History.Push(m.MessageId); 
            return m;
        }

        
    }
    //public enum StrategyDirections
    //{
    //    MoveToGoal = 1,
    //    MoveToBoard = 2,
    //    MoveToDiscoveryField = 3,
    //}
}
