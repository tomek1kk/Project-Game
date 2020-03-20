using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class ExchangeInformationResponse : MessagePayload
    {
        public int? RespondToID { get; set; }
        public IEnumerable<int> Distances { get; set; }
        public IEnumerable<string> RedTeamGoalAreaInformations { get; set; }
        public IEnumerable<string> BlueTeamGoalAreaInformations { get; set; }

        public override bool ValidateMessage()
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
