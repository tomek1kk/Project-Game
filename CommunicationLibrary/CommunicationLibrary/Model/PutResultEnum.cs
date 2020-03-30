using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Model
{
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum PutResultEnum
    {
        NormalOnGoalField,
        NormalOnNonGoalField,
        TaskField,
        ShamOnGoalArea
    }
}
