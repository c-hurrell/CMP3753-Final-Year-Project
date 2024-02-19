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
        
        public void InitFaction()
        {

        }

        public void Transaction(float amount)
        {

        }
    }
}
