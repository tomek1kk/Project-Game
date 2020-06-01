using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class ExchangeInformationGMResponse : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("respondToID")]
        public int? RespondToID { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distances")]
        public IEnumerable<int> Distances { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("redTeamGoalAreaInformations")]
        public IEnumerable<string> RedTeamGoalAreaInformations { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("blueTeamGoalAreaInformations")]
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
