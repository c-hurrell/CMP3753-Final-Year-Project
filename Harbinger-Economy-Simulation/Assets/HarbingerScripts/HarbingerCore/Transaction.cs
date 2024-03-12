using System;
using System.Collections.Generic;

namespace HarbingerCore
{
    // Implement different types of transactions?
    public abstract class Transaction
    {
        // Faction that starts the transaction e.g. vehicle docking at a station opens the transaction
        public int InitiatingFaction { get; set; }
        // Faction that is targeted for it e.g. station the vehicle docks at
        public int TargetFaction { get; set; }
        
        // <summary>
        // Total
        // + means Initiating is buying
        // - means Initiating is selling
        // <summary>
        public float Total { get; set; }
    }
    public class StationTransaction : Transaction
    {
        public int VehicleID { get; set; }  
        public List<CargoHold> CargoManifest { get; set; }
        public List<Tuple<string,float>> ResourcesTransferred { get; set; }
        
        // will be sent by a vehilce
        public List<string> ResourcesNeeded { get; set; }
    }

    public class LocationTransaction : Transaction
    {
        public int LocationID;
        // Initiating faction will be the buyer
        // Target faction will be the seller
    }
    
    // implement further transaction types here
    // e.g. ContractTransaction
}
