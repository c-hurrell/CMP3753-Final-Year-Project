using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class FactionIdentifier
    {
        public string name = "Neutral";
        public int id = 0;
    }

    [Serializable] public class Faction
    {
        public FactionIdentifier factionIdentity;

        public float credits = 1000000000f;

        public List<int> locationsOwned;
        public List<Vehicle> vehiclesOwned;
        
        public virtual void InitFaction()
        {
            
        }

        public virtual void OnTransaction(object source, BillEventArgs e)
        {
            if (e.BuyerID == factionIdentity.id) credits -= e.Currency;
            else if (e.SellerID == factionIdentity.id) credits += e.Currency;
        }
    }
}
