using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Strategies.CommunicationStrategy
{
    enum StrategyState
    {
        FindPiece,
        CheckSham,
        DestroyPiece,
        BringToGoalArea,
        ExchangeInfo,
    }
}
