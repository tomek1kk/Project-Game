using CommunicationLibrary;
using GameMaster.Game;
using CommunicationLibrary.Information;
using Serilog;

namespace GameMaster
{
    public class GameEnder
    {
        public bool endGameNotHandled = true;
        public bool lockCondition = false;
        public void GameEndHandler(Map map, IMessageSenderReceiver communicator)
        {
            string winner = map.Winner == Team.Red ? "red" : "blue";
            var message = new Message<GameEnded>()
            {
                MessagePayload = new GameEnded()
                {
                    Winner = winner
                }
            };
            foreach (Player p in map.Players.Values)
            {
                Log.Debug("Processing GameEnded message to agent: {id}", p.AgentId);
                message.AgentId = p.AgentId;
                communicator.Send(message);
            }
        }
    }
}
