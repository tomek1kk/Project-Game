using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Agent.Strategies
{
    public class SampleStrategy : Strategy
    {
        public Stack<MessageType> History { get; private set; }
        public SampleStrategy(int width, int height) : base(width, height)
        {
            History = new Stack<MessageType>();
        }

        public override Message MakeDecision(AgentInfo agent)
        {
            var last = History.Count == 0 ? MessageType.MoveRequest : History.Peek();

            if (agent.HasPiece && agent.InGoalArea())
            {
                return PutPiece();
            }
            else if (agent.HasPiece && !agent.InGoalArea())
            {
                return MoveToGoals(agent);
            }
            else if (!agent.HasPiece && agent.InGoalArea())
            {
                return BackToBoard(agent);
            }
            else if (last == MessageType.DiscoveryResponse)
            {
                return FindPiece(agent);
            }
            else if (Board[agent.Position.X, agent.Position.Y].DistToPiece == 0)
            {
                return PickPiece();
            }
            else
                return MakeDiscovery();
        }
        public override void UpdateMap(Message message, Point position)
        {
            History.Push(message.MessageId);
            base.UpdateMap(message, position);
        }

        private Message FindPiece(AgentInfo agent)
        {
            var req = new MoveRequest();
            int N = agent.Position.Y != Board.GetLength(1)
                ? Board[agent.Position.X, agent.Position.Y + 1].DistToPiece : Int32.MaxValue;
            int S = agent.Position.Y != 0
                ? Board[agent.Position.X, agent.Position.Y - 1].DistToPiece : Int32.MaxValue;
            int E = agent.Position.X != Board.GetLength(0)
                ? Board[agent.Position.X + 1, agent.Position.Y].DistToPiece : Int32.MaxValue;
            int W = agent.Position.X != 0
                ? Board[agent.Position.X - 1, agent.Position.Y].DistToPiece : Int32.MaxValue;

            int min = Math.Min(Math.Min(Math.Min(S, N), E), W);
            if (min == N)
                req.Direction = "N";
            else if (min == S)
                req.Direction = "S";
            else if (min == W)
                req.Direction = "W";
            else if (min == E)
                req.Direction = "E";
            return new Message<MoveRequest>(req);
        }
        private Message PutPiece()
        {
            return new Message<PutPieceRequest>(new PutPieceRequest());
        }
        private Message BackToBoard(AgentInfo agent)
        {
            var req = new MoveRequest();
            req.Direction = agent.GoalDirection == "N" ? "S" : "N";
            return new Message<MoveRequest>(req);
        }
        private Message PickPiece()
        {
            return new Message<PickPieceRequest>(new PickPieceRequest());
        }
        private Message MoveToGoals(AgentInfo agent)
        {
            var req = new MoveRequest();
            req.Direction = agent.GoalDirection;
            return new Message<MoveRequest>(req);
        }
        private Message MakeDiscovery()
        {
            return new Message<DiscoveryRequest>(new DiscoveryRequest());
        }

    }
    //public enum StrategyDirections
    //{
    //    MoveToGoal = 1,
    //    MoveToBoard = 2,
    //    MoveToDiscoveryField = 3,
    //}
}
