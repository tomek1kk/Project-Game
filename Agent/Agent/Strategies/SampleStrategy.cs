using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Agent.Strategies
{
    public class MyField : Field
    {
        int a;
    }
    public class SampleStrategy : Strategy
    {
        private MyField[,] customBoard;
        override public Field[,] Board { get { return customBoard; } }
        public Stack<MessageType> History { get; private set; }

        public SampleStrategy(int width, int height) : base()
        {
            History = new Stack<MessageType>();
            customBoard = new MyField[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    Board[i, j] = new MyField();
        }

        public override Message MakeDecision(AgentInfo agent)
        {
            var last = History.Count == 0 ? MessageType.MoveRequest : History.Peek();
            if (last == MessageType.MoveError)
            {
                return RandomMove();
            }
            else if (agent.HasPiece && FindUndiscoveredGoalCoordinates(agent) == (agent.Position.X, agent.Position.Y))
            {
                return PutPiece();
            }
            else if (agent.HasPiece)
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

        private Message RandomMove()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 3))
            {
                case 0:
                    return new Message<MoveRequest>(new MoveRequest() { Direction = "N" });
                case 1:
                    return new Message<MoveRequest>(new MoveRequest() { Direction = "S" });
                case 2:
                    return new Message<MoveRequest>(new MoveRequest() { Direction = "W" });
                default:
                    return new Message<MoveRequest>(new MoveRequest() { Direction = "E" });
            }

        }

        private Message FindPiece(AgentInfo agent)
        {
            var req = new MoveRequest();
            int N = agent.Position.Y != Board.GetLength(1) - 1
                ? Board[agent.Position.X, agent.Position.Y + 1].DistToPiece : Int32.MaxValue;
            int S = agent.Position.Y != 0
                ? Board[agent.Position.X, agent.Position.Y - 1].DistToPiece : Int32.MaxValue;
            int E = agent.Position.X != Board.GetLength(0) - 1
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

        private Message MakeDiscovery()
        {
            return new Message<DiscoveryRequest>(new DiscoveryRequest());
        }

        private Message MoveToGoals(AgentInfo agent)
        {
            var req = new MoveRequest();
            (int, int) closestUndiscoveredGoal = FindUndiscoveredGoalCoordinates(agent);
            (int, int) vectorToGoal = (closestUndiscoveredGoal.Item1 - agent.Position.X, closestUndiscoveredGoal.Item2 - agent.Position.Y);

            req.Direction = ChooseDirection(vectorToGoal);
            return new Message<MoveRequest>(req);
        }

        private string ChooseDirection((int, int) vector)
        {
            if (vector.Item1 < 0)
                return "W";
            if (vector.Item1 > 0)
                return "E";
            if (vector.Item2 < 0)
                return "S";
            if (vector.Item2 > 0)
                return "N";
            throw new Exception("Shouldnt be executed");
        }

        private (int, int) FindUndiscoveredGoalCoordinates(AgentInfo agentInfo)
        {
            Point position = agentInfo.Position;
            int lastRowOfGoals;
            if (agentInfo.GoalDirection == "N")
            {
                lastRowOfGoals = agentInfo.GoalArea.start;
                for (int i = lastRowOfGoals; i <= agentInfo.GoalArea.end; ++i)
                    for (int j = 0; j < Board.GetLength(0); j++)
                        if (Board[j, i].IsDiscoveredGoal == false)
                            return (j, i);
            }
            else
            {
                lastRowOfGoals = agentInfo.GoalArea.end;
                for (int i = lastRowOfGoals; i >= 0; --i)
                    for (int j = 0; j < Board.GetLength(0); j++)
                        if (Board[j, i].IsDiscoveredGoal == false)
                            return (j, i);
            }
            throw new Exception("All goals should be realized.");
        }
    }
    //public enum StrategyDirections
    //{
    //    MoveToGoal = 1,
    //    MoveToBoard = 2,
    //    MoveToDiscoveryField = 3,
    //}
}
