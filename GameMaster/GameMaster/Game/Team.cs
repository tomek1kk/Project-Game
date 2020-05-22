using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public enum Team
    {
        Red,
        Blue
    }

    public static class TeamExtensions
    {
        public static Team ToTeam(this string value)
        {
            if (value == "red")
                return Team.Red;
            else
                return Team.Blue;
        }
        public static string AsString(this Team value)
        {
            if (value == Team.Red)
                return "red";
            else
                return "blue";
        }
    }

}