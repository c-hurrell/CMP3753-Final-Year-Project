using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.WindowsMR.Input;
using Debug = UnityEngine.Debug;
using HarbingerScripts;

namespace HarbingerCore
{
    [Serializable] public struct LocationIdentifier 
    {
        public string name;
        public int locationID;
    }
    [Serializable] public class Location : EconomyActor
    {
        public event EventHandler<DockApproveEventArgs> DockApprove;
        public event EventHandler<DockApproveEventArgs> Undock;

        public event EventHandler UpdateResourceDemands;
        
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

        public List<string> inDemandResources = new List<string>();
        public List<string> producedResources = new List<string>();
        
        // Facilities
        public SpaceportFacility spaceport;
        public List<ProductionFacility> productionFacilities;

        public List<StationTransaction> dockedVehicles = new List<StationTransaction>();
        public List<StationTransaction> dockingQueue = new List<StationTransaction>();
        public int dockingCapacity;
        //public List<Vehicle> vehiclesPresent;

        private List<FactionIdentifier> _vehicles;

        public bool allowsRefueling;
        public bool allowsConfiguration;

        public override void OnEconomyAwake()
        {
            Debug.Log(identifier.name + " -> Is awake");
        }
        public override void OnEconomyUpdate()
        {
            
            Debug.Log(identifier.name + " -> Is updating");
            
            //if (true) return;
            
            // If I have vehicles waiting to dock check if they can now dock.
            if (dockingQueue.Count > 0 && dockedVehicles.Count <= dockingCapacity)
            {
                var dockedVehicle = dockingQueue[0];
                dockedVehicles.Add(dockedVehicle);
                OnDockApprove(dockedVehicle);
            }

            // If I have no docked vehicles I have no need to try and load or unload anything.
            if (dockedVehicles.Count <= 0) return;
            // I hate this code section looking into ways to improve it
            foreach (var vehicle in dockedVehicles) {
                foreach (var cargo in vehicle.CargoManifest)
                {
                    foreach (var res in inDemandResources.Where(res => cargo.resourceHeld == res))
                        LoadToInventory(res, cargo.UnloadResource(res, loadSpeed));

                    foreach (var res in vehicle.ResourcesNeeded.Where(res => producedResources.Contains(res)))
                        // UnloadFromInventory will check if there is any resource to unload then attempt to unload it.
                        LoadToInventory(res,cargo.LoadResource(res, UnloadFromInventory(res)));
                }
            }
        }

        public override void OnEconomyDestroy()
        {
            // Unsubscribe from all events and any other misc cleanup.
            
            // may handle this elsewhere
            
            // forces all vehicles to undock from the location
            foreach (var transaction in dockedVehicles)
            {
                OnUndock(transaction);
            }
            dockedVehicles.Clear();
        }
        public void LoadToInventory(string resourceName, float amount)
        {
            for (var index = 0; index < Inventory.Count; index++)
            {
                var item = Inventory[index];
                if (item.Item1 != resourceName) continue;
                var currentAmount = item.Item2;
                currentAmount += amount;
                // Tuples were a bad idea. bloody immutable. much frustration.
                Inventory[index] = new Tuple<string, float>(item.Item1, currentAmount);
            }
        }

        public float UnloadFromInventory(string resourceName)
        {
            var amount = loadSpeed;
            
            foreach (var (item1, amountHeld) in Inventory)
            {
                if (item1 != resourceName) continue;
                if (amountHeld - amount <= 0)
                {
                    return amount + (amountHeld - amount);
                }
            }
            return amount;
        }
        public virtual void DockRequest(object source, DockEventArgs e)
        {
            Debug.Log(identifier.name + " -> Dock request received");
            var newVehicle = new StationTransaction
            {
                // Information received from vehicle
                VehicleID = e.VehicleID,
                InitiatingFaction = e.FactionID,
                TargetFaction = factionID,
                CargoManifest = e.CargoManifest,
                ResourcesNeeded = e.ResourcesOnRoute,
                
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

        public virtual void OnVehicleRequest(int vehicleID, int factID, VehicleAction action)
        {
            
        }
        protected virtual void OnDockApprove(StationTransaction transaction)
        {
            Debug.Log(identifier.name +" -> Dock Approve Event Called");
            DockApprove?.Invoke(this, new DockApproveEventArgs {
                Transaction = transaction
            });
        }

        // ensure removal of transaction is done before this is called
        protected virtual void OnUndock(StationTransaction transaction)
        {
            Undock?.Invoke(this, new DockApproveEventArgs {
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

        protected virtual void OnUpdateResourceDemands()
        {
            UpdateResourceDemands?.Invoke(this, EventArgs.Empty);
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
