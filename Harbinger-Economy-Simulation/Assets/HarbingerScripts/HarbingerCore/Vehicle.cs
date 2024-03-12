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

        // This is for the faction to use.
        public bool inUse;
        
        public bool atTargetLocation;
        
        // Listen for approval
        
        public enum PathDirection
        {
            Along,
            Returning
        }
        
        // public Vector3 currentPosition;
        // May need to change this approach to account for movement of bodies in space e.g. orbit around a star?
        
        // This is important FactionManager will feed in calculated routes to this vehicle
        // Redundancy as unity will handle the physical aspect
        public List<Destination> assignedRoute;
        public Destination target;
        // PathDirection relevant?
        public PathDirection direction = PathDirection.Along;
        
        //private int _currentTarget = 1;
        
        public float fuelCapacity;
        public float amountOfFuel;

        public int cargoHoldCapacity;
        
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

        public void InitDestinations()
        {
            foreach (var dest in assignedRoute)
            {
                dest.Init();
            }
        }

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
        
        // Will be called when the vehicle wants to dock with a location
        public virtual void OnDockRequest()
        {
            Debug.Log(name +" -> Dock Request Sent");
            // possible issue when in range of multiple locations?
            status = Status.WaitingToDock;
            DockRequest?.Invoke(null, new DockEventArgs {
                VehicleID = vehicleID,
                FactionID = faction.id,
                CargoManifest = cargoHolds,
                ResourcesOnRoute = resourcesDemandedOnRoute
                // Add in fuel request
            });
        }
        // Vehicle should always be the initiating faction
        
        public virtual void OnDockApprove(object source, DockApproveEventArgs e)
        {
            Debug.Log(name + " -> Received dock approval!");
            // check that it is this vehicle receiving approval
            status = Status.Docked;
            
            if (e.Transaction.VehicleID != vehicleID || e.Transaction.InitiatingFaction != faction.id) return;
            status = Status.Docked;
        }

        public virtual void OnUndock(object source, DockApproveEventArgs e)
        {
            Debug.Log(name + " -> Received Undock approval!");
            // check that it is this vehicle being asked to undock
            if (e.Transaction.VehicleID != vehicleID || e.Transaction.InitiatingFaction != faction.id) return;
            
            // receive updated cargo
            cargoHolds = e.Transaction.CargoManifest;
            // resume travelling
            status = Status.Travelling;
        }
        #endregion


    }
    public class DockEventArgs : EventArgs
    {
        public int VehicleID { get; set; }
        public int FactionID { get; set; }
        
        public List<CargoHold> CargoManifest { get; set; }
        
        public List<string> ResourcesOnRoute { get; set; }
    }
}
