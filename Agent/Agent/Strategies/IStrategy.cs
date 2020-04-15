using System;
using System.Drawing;
using Agent.Board;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;

namespace Agent.Strategies
{
    public interface IStrategy
    {
        Message MakeDecision(AgentInfo agent);
        void UpdateMap(Message message, Point position);
        void GetInfo(ExchangeInformationGMResponse response);
    }
}

