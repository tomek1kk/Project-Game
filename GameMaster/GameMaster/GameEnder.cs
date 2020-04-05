using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using GameMaster.Configuration;
using GameMaster.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameMaster.Game;
using GameMaster.MessageHandlers;
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
        public void ErrorGameEndHandler(Map map, IMessageSenderReceiver communicator)
        {
            string winner = "error";    //what message should be if error occurs??
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
