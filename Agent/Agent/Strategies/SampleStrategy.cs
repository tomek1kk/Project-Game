using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace Agent.Strategies
{
    public class MyField : Field
    {
    }
    public class SampleStrategy : Strategy
    {
        int _decisionCount = -1;
        public Stack<MessageType> History { get; private set; }

        public SampleStrategy(int width, int height, string teamId, int goalAreaSize) : base(width, height, teamId, goalAreaSize)
        {
            History = new Stack<MessageType>();
        }

        public override Message MakeDecision(AgentInfo agent)
        {
            _decisionCount++;
            var last = History.Count == 0 ? MessageType.MoveRequest : History.Peek();
            if (agent.ExchangeInfoRequests.Count() != 0)/* && agent.ExchangeInfoRequests[0].Leader.Value)*/
            {
                var tmp = agent.ExchangeInfoRequests[0];
                agent.ExchangeInfoRequests.RemoveAt(0);
                return GiveInfo(tmp.AskingId.Value);
            }
            if (_decisionCount % 10 == 0)
            {
                var eq = new ExchangeInformationRequest();
                Random rnd = new Random();
                var allies = agent.AlliesIds.Where(x => x != agent.LeaderId);
                eq.AskedAgentId = agent.IsLeader ? allies.ElementAt(rnd.Next() % allies.Count()) : agent.LeaderId;
                return new Message<ExchangeInformationRequest>(eq);
            }

            if (last == MessageType.MoveError)
            {
                return RandomMove();
            }
            else if (agent.HasPiece && Board.FindUndiscoveredGoalCoordinates() == (agent.Position.X, agent.Position.Y))
            {
                return PutPiece();
            }
            else if (agent.HasPiece)
            {
                return MoveToGoals(agent);
            }
            else if (!agent.HasPiece && Board.InGoalArea(agent.Position))
            {
                return BackToBoard(agent);
            }
            else if (last == MessageType.DiscoveryResponse)
            {
                return FindPiece(agent);
            }
            else if (Board.Board[agent.Position.X, agent.Position.Y].DistToPiece == 0)
            {
                return PickPiece();
            }
            else
                return MakeDiscovery();
        }
        public override void UpdateMap(Message message, Point position)
        {
            if (message.MessageId != MessageType.PenaltyNotWaitedError)
                History.Push(message.MessageId);
            base.UpdateMap(message, position);
        }

        private Message GiveInfo(int AgentId)
        {
            var resp = new ExchangeInformationResponse();
            try
            {
                if (Board.GoalDirection == "N")
                    resp.RedTeamGoalAreaInformations = Board.GetGoalInfo();
                else
                    resp.BlueTeamGoalAreaInformations = Board.GetGoalInfo();
            }
            catch
            {
                string s = "DUPA";
            }
            resp.Distances = Board.GetDistances();
            resp.RespondToID = AgentId; // GM id?
            return new Message<ExchangeInformationResponse>(resp);
        }



        private Message RandomMove()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 6))
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
            int N = agent.Position.Y != Board.Board.GetLength(1) - 1
                ? Board.Board[agent.Position.X, agent.Position.Y + 1].DistToPiece : Int32.MaxValue;
            int S = agent.Position.Y != 0
                ? Board.Board[agent.Position.X, agent.Position.Y - 1].DistToPiece : Int32.MaxValue;
            int E = agent.Position.X != Board.Board.GetLength(0) - 1
                ? Board.Board[agent.Position.X + 1, agent.Position.Y].DistToPiece : Int32.MaxValue;
            int W = agent.Position.X != 0
                ? Board.Board[agent.Position.X - 1, agent.Position.Y].DistToPiece : Int32.MaxValue;

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
            req.Direction = Board.GoalDirection == "N" ? "S" : "N";
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
            (int X, int Y) closestUndiscoveredGoal = Board.FindUndiscoveredGoalCoordinates();
            (int, int) vectorToGoal = (closestUndiscoveredGoal.X - agent.Position.X, closestUndiscoveredGoal.Y - agent.Position.Y);

            req.Direction = ChooseDirection(vectorToGoal);
            return new Message<MoveRequest>(req);
        }

        private string ChooseDirection((int, int) vector)
        {
            if (vector.Item2 < 0)
                return "S";
            if (vector.Item2 > 0)
                return "N";
            if (vector.Item1 < 0)
                return "W";
            if (vector.Item1 > 0)
                return "E";
            throw new Exception("Shouldnt be executed");
        }



    }
    //public enum StrategyDirections
    //{
    //    MoveToGoal = 1,
    //    MoveToBoard = 2,
    //    MoveToDiscoveryField = 3,
    //}
}
