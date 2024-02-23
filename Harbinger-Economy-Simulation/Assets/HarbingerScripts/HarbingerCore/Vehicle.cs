using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarbingerScripts;
// Change to generic vector3?
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class Vehicle
    {
        public string name;

        public int factionID;
        
        public enum PathDirection
        {
            Along,
            Returning
        }
        
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

        //public float averageUsage;

        public float estimatedRange;

        public LocationIdentifier atLocation;
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

        public void OnVehicleDocked(object sender, EventArgs e)
        {
            Debug.Log("VehicleDocked");
        }


    }
}
