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

    public class DockApproveEventArgs : EventArgs
    {
        public Transaction Transaction { get; set; }
    }
    public class BillEventArgs : EventArgs
    {
        // Change of currency
        public float Currency { get; set; }
        // Identifying arguments to identify relevant faction?
        public int BuyerID { get; set; }
        public int SellerID { get; set; }
    }
    [Serializable] public class Location
    {
        public static event EventHandler<DockApproveEventArgs> DockApprove;
        public static event EventHandler<DockApproveEventArgs> UndockApprove; 
        
        //public static event EventHandler<TransactionEventArgs> Transaction;
        public static event EventHandler<BillEventArgs> Bill;

        public LocationIdentifier identifier;

        public int factionID = 0;
        
        public Vector3 position;

        public RegionIdentifier belongsToRegion;

        public float loadSpeed;

        public List<Resource> resourceMarket;

        // stores resource name and the amount held.
        public List<Tuple<string, float>> inventory = new List<Tuple<string, float>>();

        public List<ResourceProduction> resourceProductions;

        public List<Transaction> dockedVehicles = new List<Transaction>();
        public List<Transaction> dockingQueue = new List<Transaction>();
        public int dockingCapacity;
        //public List<Vehicle> vehiclesPresent;

        private List<FactionIdentifier> _vehicles;

        public bool allowsRefueling;

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
            var newVehicle = new Transaction
            {
                VehicleID = e.VehicleID,
                FactionID = e.FactionID,
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
        
        protected virtual void OnDockApprove(Transaction transaction)
        {
            DockApprove?.Invoke(this, new DockApproveEventArgs {
                Transaction = transaction
            });
        }

        protected virtual void OnUndockApprove(Transaction transaction)
        {
            UndockApprove?.Invoke(this, new DockApproveEventArgs
            {
                Transaction = transaction
            });
        }

        protected virtual void OnBill(BillEventArgs e)
        {
            Bill?.Invoke(null, e);
        }
    }
    
    // REPLACE THIS YOU DUMBASS
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
