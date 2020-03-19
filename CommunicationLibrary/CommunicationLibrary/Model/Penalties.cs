using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Model
{
    public class Penalties
    {
        public string Move { get; set; }
        public string CheckForSham { get; set; }
        public string Discovery { get; set; }
        public string DestroyPiece { get; set; }
        public string PutPiece { get; set; }
        public string InformationExchange { get; set; }
    }
}
