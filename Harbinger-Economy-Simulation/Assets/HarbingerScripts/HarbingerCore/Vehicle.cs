using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// Change to generic vector3?
using UnityEngine;

namespace HarbingerCore
{
    public partial class DockEventArgs : EventArgs
    {
        public int VehicleID { get; set; }
        public int FactionID { get; set; }
        
        public List<CargoHold> CargoManifest { get; set; }
    }
    [Serializable] public class Vehicle
    {
        public string name;
        public int vehicleID;
        public FactionIdentifier faction;
        
        public static event EventHandler DockRequest;
        public static event EventHandler UndockRequest;
        
        
        // Listen for approval
        
        public enum PathDirection
        {
            Along,
            Returning
        }
        
        // Do I need to update current position?
        // -- > no
        // Dont care about this anymore
        // public Vector3 currentPosition;
        // May need to change this approach to account for movement of bodies in space e.g. orbit around a star?
        
        // This is important
        public List<Vector3> route;
        public PathDirection direction = PathDirection.Along;
        
        //private int _currentTarget = 1;
        
        public float fuelCapacity;
        public float amountOfFuel;

        public float baseUsagePerTick;
        public float modifier;

        //public float averageUsage;

        public float estimatedRange;

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
            });
        }
        public virtual void OnDockApprove(object source, DockApproveEventArgs e)
        {
            if(e.Transaction.VehicleID == vehicleID && e.Transaction.FactionID == faction.id)
                status = Status.Docked;
        }

        public virtual void OnUndockApprove(object source, DockApproveEventArgs e)
        {
            if (e.Transaction.VehicleID != vehicleID || e.Transaction.FactionID != faction.id) return;
            cargoHolds = e.Transaction.CargoManifest;
            status = Status.Travelling;
        }
        #endregion
    }
}
