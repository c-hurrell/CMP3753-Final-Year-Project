using System;
using System.Collections;
using System.Collections.Generic;
using HarbingerScripts;
using UnityEngine;

namespace HarbingerCore
{
    // No longer in use
    [Serializable] public struct RegionIdentifier
    {
        // Unique identifies for each region
        public int regionID;
        public string name;
        public Vector3 regionCentre;
    }
    [Serializable] public class Region
    {
        public RegionIdentifier regionIdentity;

        public List<Resource> resourceMarket;

        public List<int> locations;

        public float population;

        public float influenceRange;
    }
}
