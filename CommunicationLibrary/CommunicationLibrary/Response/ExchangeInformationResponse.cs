using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationLibrary.Response
{
    class ExchangeInformationResponse : IMessage
    {
        public int? RespondToID { get; set; }
        public List<int> Distances { get; set; }
        public List<string> RedTeamGoalAreaInformations { get; set; }
        public List<string> BlueTeamGoalAreaInformations { get; set; }

        public bool ValidateMessage()
        {
            if (RespondToID == null)
                return false;
            if (Distances == null)
                return false;
            if (RedTeamGoalAreaInformations == null)
                return false;
            if (BlueTeamGoalAreaInformations == null)
                return false;
            return true;
        }
    }
}
