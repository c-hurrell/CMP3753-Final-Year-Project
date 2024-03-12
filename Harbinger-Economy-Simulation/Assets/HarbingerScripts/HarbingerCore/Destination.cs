using System;
using System.Collections;
using System.Collections.Generic;
using HarbingerScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable]
    public class Destination
    {
        // Reference to its location to fetch relevant information
        public GameObject location;
        public Vector3 position;
        
        // The resources this location needs
        public List<string> inDemandResource;
        // The resource this location produces
        public List<string> producedResources;
        
        // Assigned by the route as rules to follow
        public List<string> loadResource;
        public List<string> unloadResource;
        
        // Tells the vehicle to refuel here
        public bool refuel;
        
        // The configuration the vehicle should have when it leaves this location
        public List<ResourceType> cargoHoldConfiguration;

        public void Init()
        {
            position = location.transform.position;
            inDemandResource = location.GetComponent<LocationManager>().location.inDemandResources;
            producedResources = location.GetComponent<LocationManager>().location.producedResources;
        }
    }
}
