using System;
using System.Drawing;
using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Request;

namespace Agent.Strategies
{
    public interface IStrategy
    {
        Message MakeDecision(AgentInfo agent);
        void UpdateMap(Message message, Point position);
    }
}

