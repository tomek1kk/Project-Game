using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class DiscoveryResponse : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("distanceFromCurrent")]
        public int? DistanceFromCurrent { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceN")]
        public int? DistanceN { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceNE")]
        public int? DistanceNE { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceE")]
        public int? DistanceE { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceSE")]
        public int? DistanceSE { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceS")]
        public int? DistanceS { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceSW")]
        public int? DistanceSW { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceW")]
        public int? DistanceW { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("distanceNW")]
        public int? DistanceNW { get; set; }

        public override bool ValidateMessage()
        {
            if (DistanceFromCurrent == null)
                return false;
            return true;
        }
    }
}
