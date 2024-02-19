using System;
using System.Collections.Generic;
using System.Linq;
using HarbingerCore;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class Location
    {
        public int locationID;
        public string name;

        public int factionID = 0;
        
        public Vector3 position;

        public RegionIdentifier belongsToRegion;

        public float loadSpeed;

        public List<Resource> resources;

        public List<ResourceProduction> resourceProductions;

        public int dockingCapacity;
        public List<global::HarbingerCore.Vehicle> vehiclesPresent;

        public bool allowsRefueling;

        public void UpdateLocation()
        {
            foreach (var cargoHold in from vehicle in vehiclesPresent from cargoHold in vehicle.cargoHolds from resourceProduction in resourceProductions 
                     where resourceProduction.resourcesRequired.Contains(cargoHold.resourceHeld) select cargoHold)
            {
                
            }

            foreach (var vehicle in vehiclesPresent)
            {
                foreach (var cargoHold in from cargoHold in vehicle.cargoHolds from resourceProduction in resourceProductions select cargoHold)
                {
                    UpdateAmountResource(cargoHold.resourceHeld, 
                        cargoHold.UnloadResource(cargoHold.resourceHeld, loadSpeed),
                        vehicle.factionID);
                }
            }
        }
        public void UpdateAmountResource(string resourceName, float amount, int faction)
        {
            foreach (var resource in resources.Where(resource => resource.resourceInfo.name == resourceName))
            {
                resource.stored += amount;
                
            }
        }
    }
    [Serializable] public class ResourceProduction
    {
        // 
        public string name = "Production Type";
        public List<string> resourcesProduced;
        // amount produced each per second
        // Struct for resources productions
        public List<float> amountProduced;
        public List<float> exportStored;

        // 
        public List<string> resourcesRequired;
        // amount required per second
        public List<float> amountRequired;
        public List<float> importStored;
    }
}
