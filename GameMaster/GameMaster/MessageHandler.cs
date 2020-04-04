﻿using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.MessageHandlers;
using GameMaster.Configuration;
using Serilog;

namespace GameMaster.Game
{
    public abstract class MessageHandler : IMessageHandler
    {
        protected int _agentId;
        protected bool _hasTimePenalty = false;
        private void BaseReadMessage(Message message)
        {
            _agentId = (int)message.AgentId;
            ReadMessage(message.GetPayload());
        }
        public Message ProcessRequest(Map map, Message message, GMConfiguration configuration)
        {
            Log.Information("Processing message from agent {Agent}", _agentId);
            Log.Debug("Received message content: {@Message}", message);
            ClearHandler();
            BaseReadMessage(message);
            CheckAgentPenaltyIfNeeded(map);
            if (_hasTimePenalty)
                return GetPenaltyError(map);
            if (CheckRequest(map))
                Execute(map);
            SetTimeout(configuration, map);
            var response = GetResponse(map);
            response.AgentId = _agentId;
            Log.Debug("Prepared response: {@Response}", response);
            return response;
        }
        protected abstract bool CheckRequest(Map map);
        protected abstract Message GetResponse(Map map);
        protected abstract void Execute(Map map);
        protected abstract void ReadMessage(MessagePayload payload);
        protected abstract void SetTimeout(GMConfiguration config, Map map);
        protected abstract void CheckAgentPenaltyIfNeeded(Map map);
        protected abstract void ClearHandler();
        private Message GetPenaltyError(Map map)
        {
            return new Message<PenaltyNotWaitedError>()
            {
                AgentId = _agentId,
                MessagePayload = new PenaltyNotWaitedError()
                {
                    WaitUntill = map.GetPlayerById(_agentId).LockedTill
                }
            };
        }
        protected void CheckIfAgentHasPenalty(Map map)
        {
            _hasTimePenalty = map.GetPlayerById(_agentId).IsLocked;
        }
    }
}
