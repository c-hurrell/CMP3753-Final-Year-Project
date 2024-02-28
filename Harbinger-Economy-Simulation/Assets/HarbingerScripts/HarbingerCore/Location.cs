using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public struct LocationIdentifier 
    {
        public string name;
        public int locationID;
    }
    [Serializable] public class Location
    {
        public event EventHandler<DockApproveEventArgs> DockApprove;
        public event EventHandler<DockApproveEventArgs> Undock; 
        
        //public static event EventHandler<TransactionEventArgs> Transaction;
        public static event EventHandler<BillEventArgs> Bill;

        public LocationIdentifier identifier;

        public int factionID = 0;
        
        public Vector3 position;

        public RegionIdentifier belongsToRegion;

        public float loadSpeed;

        public List<Resource> resourceMarket;

        // stores resource name and the amount held.
        public List<Tuple<string, float>> Inventory = new List<Tuple<string, float>>();
        
        // Facilities
        public SpaceportFacility spaceport;
        public List<ProductionFacility> productionFacilities;

        public List<StationTransaction> dockedVehicles = new List<StationTransaction>();
        public List<StationTransaction> dockingQueue = new List<StationTransaction>();
        public int dockingCapacity;
        //public List<Vehicle> vehiclesPresent;

        private List<FactionIdentifier> _vehicles;

        public bool allowsRefueling;

        public void InitLocation()
        {
            
        }
        public void UpdateLocation()
        {
            if (dockingQueue.Count > 0 && dockedVehicles.Count <= dockingCapacity)
            {
                var dockedVehicle = dockingQueue[0];
                dockedVehicles.Add(dockedVehicle);
                OnDockApprove(dockedVehicle);
            }
            // Implement Unloading Logic
            foreach (var vehicle in dockedVehicles)
            {
                foreach (var cargo in vehicle.CargoManifest)
                {
                    
                }
            }
            
        }
        public void UpdateAmountResource(string resourceName, float amount, int faction)
        {
            
        }
        public virtual void DockRequest(object source, DockEventArgs e)
        {
            var newVehicle = new StationTransaction
            {
                VehicleID = e.VehicleID,
                InitiatingFaction = e.FactionID,
                TargetFaction = factionID,
                CargoManifest = e.CargoManifest,
                ResourcesTransferred = null,
                Total = 0
            };

            if (dockedVehicles.Count >= dockingCapacity - 1)
            {
                dockingQueue.Add(newVehicle);
            }
            else
            {
                dockedVehicles.Add(newVehicle);
                OnDockApprove(newVehicle);
            }
        }
        
        protected virtual void OnDockApprove(StationTransaction transaction)
        {
            DockApprove?.Invoke(this, new DockApproveEventArgs {
                Transaction = transaction
            });
        }

        protected virtual void OnUndock(StationTransaction transaction)
        {
            Undock?.Invoke(this, new DockApproveEventArgs
            {
                Transaction = transaction
            });
        }

        protected virtual void OnBill(BillEventArgs e)
        {
            Bill?.Invoke(null, e);
        }

        public void OnDestroy()
        {
            // unsubscribe from all events.
        }
    }
    public class DockApproveEventArgs : EventArgs
    {
        public StationTransaction Transaction { get; set; }
    }
    public class BillEventArgs : EventArgs
    {
        public int LocationID { get; set; }
        // Change of currency
        public float Currency { get; set; }
        // Identifying arguments to identify relevant faction?
        public int FactionID { get; set; }
        public int VehicleID { get; set; }
    }
}
