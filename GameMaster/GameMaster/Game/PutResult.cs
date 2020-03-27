using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum PutResult
    {
        NormalOnGoalField,
        NormalOnNonGoalField,
        TaskField,
        ShamOnGoalArea
    }
}
