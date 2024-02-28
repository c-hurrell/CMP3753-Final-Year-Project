using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// Change to generic vector3?
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class Vehicle
    {
        public string name;
        public int vehicleID;
        public FactionIdentifier faction;
        
        // Planned change :
        // Instead of static event have a location subscribe to this event when a vehicle comes into range that way its only listening for vehicles in its range.
        public event EventHandler<DockEventArgs> DockRequest;
        
        // Redundant as Undocking is handled by the location
        //public static event EventHandler UndockRequest;
        
        
        // Listen for approval
        
        public enum PathDirection
        {
            Along,
            Returning
        }
        
        // public Vector3 currentPosition;
        // May need to change this approach to account for movement of bodies in space e.g. orbit around a star?
        
        // This is important FactionManager will feed in calculated routes to this vehicle
        public List<Vector3> route;
        public PathDirection direction = PathDirection.Along;
        
        //private int _currentTarget = 1;
        
        public float fuelCapacity;
        public float amountOfFuel;
        
        // base usage is the usage to maintain vehicle speed etc. whilst docked a vehicle would be using these.
        public float baseUsagePerTick;
        // modifier will be a total determined by the acceleration or deceleration of the vehicle which will increase the fuel consumption
        public float modifier;
        // Explanation :
        // In space there is a lack of resistive forces so most fuel that would be needed is to maintain power functionality and keep "engines" in a standby state

        public float maxSpeed;
        public float maxAcceleration;

        
        //public float averageUsage;
        
        
        // base usage will help the vehicle estimate how far they can really go 
        public float estimatedRange;
        // threshold which the vehicle decides it needs to get fuel in order to continue its journey 
        [SerializeField] private float rangeThreshold = 10.0f;

        public List<string> resourcesDemandedOnRoute = new List<string>();
        public LocationIdentifier atLocation;

        public enum Status
        {
            Travelling,
            Docked,
            WaitingToDock
        }

        public Status status;

        public List<CargoHold> cargoHolds;

        public void InitVehicle()
        {
            
        }
        // Redundant function;
        public void VehicleUpdate()
        {
            // Could change it so OnCollision in Unity handles the docking??
            // if (currentPosition == path[_currentTarget])
            // {
            //     
            // }
            // Implement Movement mechanics?
        }
        // Amount inputted will be loaded and the return is the value loaded to be taken away from the total
        public float Refuel(float amount)
        {
            // If fuel is at capacity don't load any fuel
            if (amountOfFuel >= fuelCapacity)
            {
                status = Status.Docked;
                return 0;
            }
            
            // If the amount loaded doesn't go over capacity load all fuel
            if (!(amountOfFuel + amount >= fuelCapacity)) return amount;
            
            // Else load whatever fuel is remaining
            var total = amount - (amountOfFuel + amount - fuelCapacity);
            return total;

        }
        public double CalculateDistance(Vector3 start, Vector3 end)
        {
            var xDis = start.x - end.x;
            var yDis = start.y - end.y;

            var distance = Math.Sqrt(Math.Pow(xDis, 2) + Math.Pow(yDis, 2));
            
            return distance;
        }
        public void OnDestroy()
        {
            // unsubscribe from all events.
        }
        
        // =========================
        // ==== EVENT FUNCTIONS ====
        // =========================
        #region Dock Event Functions
        // Dock Event Functions
        protected virtual void OnDockRequest()
        {
            status = Status.WaitingToDock;
            DockRequest?.Invoke(null, new DockEventArgs {
                VehicleID = vehicleID,
                FactionID = faction.id,
                CargoManifest = cargoHolds
                // Add in sending loadable cargo?
                // Add in fuel request
            });
        }
        // Vehicle should always be the initiating faction
        
        public virtual void OnDockApprove(object source, DockApproveEventArgs e)
        {
            if (e.Transaction.VehicleID != vehicleID || e.Transaction.InitiatingFaction != faction.id) return;
            
            status = Status.Docked;
        }

        public virtual void OnUndock(object source, DockApproveEventArgs e)
        {
            if (e.Transaction.VehicleID != vehicleID || e.Transaction.InitiatingFaction != faction.id) return;
            
            cargoHolds = e.Transaction.CargoManifest;
            status = Status.Travelling;
        }
        #endregion


    }
    public class DockEventArgs : EventArgs
    {
        public int VehicleID { get; set; }
        public int FactionID { get; set; }
        
        public List<CargoHold> CargoManifest { get; set; }
    }
}
