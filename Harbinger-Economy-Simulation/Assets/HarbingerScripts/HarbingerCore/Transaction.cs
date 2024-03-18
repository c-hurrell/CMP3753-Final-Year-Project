using System;
using System.Collections.Generic;

namespace HarbingerCore
{
    // Implement different types of transactions?
    public class Transaction
    {
        // Faction that starts the transaction e.g. vehicle docking at a station opens the transaction
        public int InitiatingFaction { get; set; }
        // Faction that is targeted for it e.g. station the vehicle docks at
        public int TargetFaction { get; set; }
        
        // Used for receiving checks and identifying where its gone
        public int InitiatorID { get; set; }
        
        public int TargetID { get; set; }

        public TransactionType Type { get; set; }
        
        // <summary>
        // Total
        // + means Initiating is buying
        // - means Initiating is selling
        // <summary>
        public float Total { get; set; }
        
        // For Station Type
        public List<Tuple<string, float>> ResourcesTransferred { get; set; }
        public List<string> ResourcesNeeded { get; set; }
        public float fuelRequired = .0f;
    }
    
    // implement further transaction types here
    // e.g. ContractTransaction
    public enum TransactionType
    {
        Basic,
        Location
    }
        
}
