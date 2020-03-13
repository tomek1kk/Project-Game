using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Response
{
    public class DiscoveryResponse : Message
    {
        public int? DistanceFromCurrent { get; set; }
        public int? DistanceN { get; set; }
        public int? DistanceNE { get; set; }
        public int? DistanceE { get; set; }
        public int? DistanceSE { get; set; }
        public int? DistanceS { get; set; }
        public int? DistanceSW { get; set; }
        public int? DistanceW { get; set; }
        public int? DistanceNW { get; set; }

        public override bool ValidateMessage()
        {
            if (DistanceFromCurrent == null || DistanceN == null || DistanceNE == null ||
                DistanceE == null || DistanceSE == null || DistanceS == null ||
                DistanceSW == null || DistanceW == null || DistanceNW == null)
                return false;
            return true;
        }
    }
}
