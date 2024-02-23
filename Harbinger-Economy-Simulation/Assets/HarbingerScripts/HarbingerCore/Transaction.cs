using System;
using System.Collections.Generic;

namespace HarbingerCore
{
    public class Transaction
    {
        public int VehicleID { get; set; }
        public int FactionID { get; set; }
        
        public List<CargoHold> CargoManifest { get; set; }
        public List<Tuple<string, float>> ResourcesTransferred { get; set; }
        public float Total { get; set; }
    }
}
