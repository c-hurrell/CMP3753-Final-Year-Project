
using System;
using System.Collections.Generic;
using System.Linq;
using HarbingerScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Cache = Unity.VisualScripting.Cache;

namespace HarbingerScripts
{
    [Serializable] public class Resource
    {
        // Resource information
        public string name;

        public ResourceType type;
        // The base value globally
        public float baseValue; 
        // The current value due to supply/demand
        public float currentValue;
        // Amount stored locally
        public float stored; 
        // Amount needed locally
        public float demand;
    }

    [Serializable] public class ResourceProduction
    {
        // 
        public string name = "Production Type";
        public List<string> resourcesProduced;
        // amount produced each per second
        public List<float> amountProduced;
        public List<float> exportStored;

        public List<string> resourcesRequired;
        // amount required per second
        public List<float> amountRequired;
        public List<float> importStored;
    }
    [Serializable] public class RegionIdentifier
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
        public List<Vehicle> vehiclesPresent;

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
            foreach (var resource in resources.Where(resource => resource.name == resourceName))
            {
                resource.stored += amount;
                
            }
        }
        
        
    }

    [Serializable] public class Vehicle
    {
        public string name;

        public int factionID;
        
        // Do I need to update current position?
        // -- > no
        public Vector3 currentPosition;
        // May need to change this approach to account for movement of bodies in space e.g. orbit around a star?
        public List<Vector3> path;

        public PathDirection direction = PathDirection.Along;
        

        private int _currentTarget = 1;
        
        public float fuelCapacity;
        public float amountOfFuel;

        public float baseUsagePerTick;
        public float modifier;

        public float averageUsage;

        public float estimatedRange;

        public bool docked;
        public bool isLoading;
        public bool isWaitingToDock;

        public List<CargoHold> cargoHolds;

        public void VehicleUpdate()
        {
            if (docked)
            {
                foreach (var cargoHold in cargoHolds.Where(cargoHold => cargoHold.isLoading == true))
                {
                    isLoading = true;
                    break;
                }
                
            }
            
            // Could change it so OnCollision in Unity handles the docking??
            if (currentPosition == path[_currentTarget])
            {
                
            }
            // Implement Movement mechanics?
        }
        
        // If docking successful return true
        public bool DockVehicle(Location location)
        {
            if (location.vehiclesPresent.Count >= location.dockingCapacity) return false;
            location.vehiclesPresent.Add(this);
            return true;
        }
        
        // If undocking successful return true
        public bool UndockVehicle(Location location)
        {
            // If vehicle is still loading or unloading it can't undock.
            if (isLoading) return false;
            location.vehiclesPresent.Remove(this);
            return true;
        }
        
        // Amount inputted will be loaded and the return is the value loaded to be taken away from the total
        public float Refuel(float amount)
        {
            // If fuel is at capacity don't load any fuel
            if (amountOfFuel >= fuelCapacity)
            {
                isLoading = false;
                return 0;
            }
            
            // If the amount loaded goes over capacity only add to capacity
            if (amountOfFuel + amount >= fuelCapacity)
            {
                var total = amount - (amountOfFuel + amount - fuelCapacity);
                return total;
            }
            
            // If no issues load all fuel
            isLoading = true;
            return amount;
        }

        public void SetVehiclePath(List<Vector3> newPath)
        {
            path = newPath;
            
            // Sets new target
            _currentTarget = 0;
            
            var closestDist = CalculateDistance(currentPosition, path[0]);
            
            for (var i = 0; i < path.Count; i++)
            {
                var distance = CalculateDistance(currentPosition, path[i]);
                if (distance < closestDist)
                {
                    _currentTarget = i;
                }
            }
        }

        public double CalculateDistance(Vector3 start, Vector3 end)
        {
            var xDis = start.x - end.x;
            var yDis = start.y - end.y;

            var distance = Math.Sqrt(Math.Pow(xDis, 2) + Math.Pow(yDis, 2));
            
            return distance;
        }
    }

    [Serializable] public class CargoHold
    {
        public float capacity;
        public ResourceType holdsType;
        
        public string resourceHeld;
        public float amountHeld;

        public bool isLoading;
        
        // Returns the amount that is loaded into the hold. This meaning the amount should be the amount attempted to load.
        public float LoadResource(string resourceName, float amount)
        {
            // If a different resource is held don't load resource
            if (resourceName != resourceHeld && amountHeld > 0) {
                isLoading = false;
                return 0;
            }

            isLoading = true;
            
            resourceHeld = resourceName;
            
            if (amountHeld + amount >= capacity)
            {
                var loaded =  amount - ((amountHeld + amount) - capacity);
                amountHeld += loaded;
                isLoading = false;
                return loaded;
            }
            
            amountHeld += amount;

            return amount;
        }

        // Returns the amount that is unloaded out of the hold. This is the amount should be attempted to unloaded each update
        public float UnloadResource(string resourceName, float amount)
        {
            // If you are attempting to unload a resource that isn't on the ship don't unload
            if (resourceName != resourceHeld || amountHeld <= 0) {
                isLoading = false;
                return 0;
            }

            isLoading = true;

            if (amountHeld - amount <= 0)
            {
                var unloaded = amount - (amountHeld - amount);
                amountHeld -= unloaded;
                isLoading = false;
                return unloaded;
            }

            amountHeld -= amount;
            
            return amount;
        }
    }

    public enum ResourceType
    {
        Solid,
        Liquid,
        Gas
    }

    public enum PathDirection
    {
        Along,
        Returning
    }

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