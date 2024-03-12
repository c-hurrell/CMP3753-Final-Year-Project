using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarbingerScripts;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class Route
    {
        public string routeName;
        // Best way I can think of easy access to locations
        //public List<GameObject> destinations;

        public List<Destination> destinations;

        public int vehiclesOnRoute;
        
        // Used for UI information later...?
        public int factionID;
        
        // route statistics?...
        

        public List<string> resourcesRequired;
        
        public void Init()
        {
            foreach (var destination in destinations)
            {
                // If not a location remove this destination
                if (!destination.location.CompareTag("Location")) {
                    Debug.LogWarning("Destination is not a location!");
                    destinations.Remove(destination);
                    continue;
                }
                destination.Init();
                
                UpdateResourceDemand(destination);
                
                // Changing this so that instead destination receives the event and then sends its own event in which route will then update its demands.
                destination.location.GetComponent<LocationManager>().location.UpdateResourceDemands += OnUpdateResourceDemands;
            }
        }
        
        public void Destroy()
        {
            foreach (var destination in destinations)
            {
                destination.location.GetComponent<LocationManager>().location.UpdateResourceDemands -= OnUpdateResourceDemands;
            }
        }

        
        // Will need a rework
        public void OnUpdateResourceDemands(object source, EventArgs e)
        {
            foreach (var destination in destinations)
            {
                UpdateResourceDemand(destination);
            }
        }

        public void UpdateResourceDemand(Destination dest)
        {
            foreach(var res in dest.location.GetComponent<LocationManager>().location.inDemandResources.Where(res => !resourcesRequired.Contains(res)))
                resourcesRequired.Add(res);
        }

    }
}
