using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using Agent.Strategies.CommunicationStrategy;
using Serilog;

namespace Agent.Strategies
{
    public class CommunicateStrategy : Strategy
    {
        public Stack<MessageType> History { get; private set; }
        private StrategyState strategyState = StrategyState.FindPiece;
        private Dictionary<int, int> agentsToAskForLeader = new Dictionary<int, int>();//for non leader not used

        public CommunicateStrategy(int width, int height, string teamId, int goalAreaSize) : base(width, height, teamId, goalAreaSize)
        {
            History = new Stack<MessageType>();
            History.Push(MessageType.MoveRequest);
        }

        public Message MakeDecisionNonLeader(AgentInfo agent)
        {
            Message msg = null;
            if (agent.AlliesIds.Count() >= 1 && agent.IsLeader)
            {
                foreach (var v in agentsToAskForLeader.Keys.ToList())
                {
                    agentsToAskForLeader[v]++;
                }
                var firstOrNull = agentsToAskForLeader.FirstOrDefault(x => x.Value > Math.Min(Math.Max((Board.NumberOfFilledRows()*2),3),8));
                if (firstOrNull.Key != 0)
                {
                    agentsToAskForLeader.Remove(firstOrNull.Key);
                    return LogMessage(new Message<ExchangeInformationRequest>(new ExchangeInformationRequest() { AskedAgentId = firstOrNull.Key }));
                }
            }
            if (agent.ExchangeInfoRequests.Count() != 0)
            {
                var tmp = agent.ExchangeInfoRequests[0];
                agent.ExchangeInfoRequests.RemoveAll(x => x.AskingId == tmp.AskingId);
                if (agent.AlliesIds.Count() >= 1 && agent.IsLeader)
                    agentsToAskForLeader[tmp.AskingId.Value] = 0;

                return LogMessage(GiveInfo(tmp.AskingId.Value));
            }

            MessageType last = History.Peek();
            if (last == MessageType.MoveError)
                return LogMessage(RandomMove());

            if (last == MessageType.PutPieceError)
            {
                strategyState = StrategyState.FindPiece;
                return LogMessage(RandomMove());//dont know when and what to do
            }
            switch (strategyState)
            {
                case StrategyState.FindPiece:
                    if (agent.HasPiece)
                        return LogMessage(new Message<CheckHoldedPieceRequest>(new CheckHoldedPieceRequest()));
                    else
                        return LogMessage(FindPieceAction(agent));
                case StrategyState.CheckSham:
                    return LogMessage(new Message<CheckHoldedPieceRequest>(new CheckHoldedPieceRequest()));
                case StrategyState.BringToGoalArea:
                    if (!agent.HasPiece)
                    {
                        strategyState = StrategyState.FindPiece;
                        return LogMessage(FindPiece(agent));
                    }
                    if (!Board.InGoalArea(agent.Position))
                        return LogMessage(MoveToGoals(agent));

                    var nearest = Board.FindUndiscoveredGoalCoordinates(agent.Position);
                    int distance = Math.Abs(nearest.Item1 - agent.Position.X) + Math.Abs(nearest.Item2 - agent.Position.Y);
                    if (distance > 1)
                    {
                        return LogMessage(MoveToGoals(agent));
                    }
                    else if (agent.AlliesIds.Count() >= 1 && !agent.IsLeader)
                    {
                        strategyState = StrategyState.ExchangeInfo;
                        return LogMessage(new Message<ExchangeInformationRequest>(new ExchangeInformationRequest() { AskedAgentId = agent.LeaderId }));
                    }
                    else if (nearest != (agent.Position.X, agent.Position.Y))
                    {
                        return LogMessage(MoveToGoals(agent));
                    }
                    else
                        return LogMessage(new Message<PutPieceRequest>(new PutPieceRequest()));
                case StrategyState.ExchangeInfo:
                    if (!agent.HasPiece)
                    {
                        strategyState = StrategyState.BringToGoalArea;
                        return LogMessage(FindPieceAction(agent));
                    }
                    if (Board.FindUndiscoveredGoalCoordinates(agent.Position) != (agent.Position.X, agent.Position.Y))//should always be executed
                        return LogMessage(MoveToGoals(agent));
                    else
                        return LogMessage(new Message<PutPieceRequest>(new PutPieceRequest()));
                case StrategyState.DestroyPiece:
                    return LogMessage(new Message<DestroyPieceRequest>(new DestroyPieceRequest()));
            }
            return msg;//never
        }
        private Message LogMessage(Message m)
        {
            Log.Information("Send {MessageId} current state ", m.MessageId);
            return m;
        }
        private Message FindPieceAction(AgentInfo agent)
        {
            if (Board.InGoalArea(agent.Position))
                return BackToBoard(agent);
            MessageType last = History.Peek();
            if (Board.Board[agent.Position.X, agent.Position.Y].DistToPiece == 0)
                return new Message<PickPieceRequest>(new PickPieceRequest());
            if (last == MessageType.DiscoveryResponse)
                return FindPiece(agent);

            return MakeDiscovery();
        }


        public override Message MakeDecision(AgentInfo agent)
        {
            return MakeDecisionNonLeader(agent);
        }
        public override void UpdateMap(Message message, Point position)
        {
            if (message.MessageId != MessageType.PenaltyNotWaitedError)
                History.Push(message.MessageId);
            switch (message.MessageId)
            {
                case MessageType.CheckHoldedPieceResponse:
                    if (((CheckHoldedPieceResponse)message.GetPayload()).Sham == true)
                        strategyState = StrategyState.DestroyPiece;
                    else
                        strategyState = StrategyState.BringToGoalArea;
                    break;
                case MessageType.DestroyPieceResponse:
                    strategyState = StrategyState.FindPiece;
                    break;
                case MessageType.PickPieceResponse:
                    strategyState = StrategyState.CheckSham;
                    break;
                case MessageType.PutPieceResponse:
                    strategyState = StrategyState.FindPiece;
                    break;
                case MessageType.ExchangeInformationGMResponse:
                    strategyState = StrategyState.ExchangeInfo;
                    break;
            }

            base.UpdateMap(message, position);
        }

        private Message GiveInfo(int AgentId)
        {
            var resp = new ExchangeInformationResponse();
            resp.BlueTeamGoalAreaInformations = resp.RedTeamGoalAreaInformations = new string[0];
            try
            {
                if (Board.GoalDirection == "N")
                    resp.RedTeamGoalAreaInformations = Board.GetGoalInfo();
                else
                    resp.BlueTeamGoalAreaInformations = Board.GetGoalInfo();
            }
            catch
            {
            }
            resp.Distances = Board.GetDistances();
            resp.RespondToID = AgentId; // GM id?
            return new Message<ExchangeInformationResponse>(resp);
        }



        private Message RandomMove()
        {
            Random rnd = new Random();
            if (Board.GoalDirection == "N")
                switch (rnd.Next(0, 6))
                {
                    case 0:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "N" });
                    case 1:
                    case 2:
                    case 3:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "S" });
                    case 4:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "W" });
                    default:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "E" });
                }
            else
                switch (rnd.Next(0, 6))
                {
                    case 0:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "S" });
                    case 1:
                    case 2:
                    case 3:
                        return new Message<MoveRequest>(new MoveRequest() { Direction = "N" });
                    case 4:
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
            (int X, int Y) closestUndiscoveredGoal = Board.FindUndiscoveredGoalCoordinates(agent.Position);
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
