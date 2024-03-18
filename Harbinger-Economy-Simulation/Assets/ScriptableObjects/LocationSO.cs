using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarbingerCore;
using UnityEngine;

public class LocationSO : EconomyActorSO
{
    [SerializeField] public string locationName;
    [SerializeField] public int id;

    // Default faction for a location is unowned -> Can be set from SO.
    public int factionID = 0;
    public Vector3 position;
    
    // Transaction Handler
    [SerializeField] private TransactionHandlerSO tH;
    // Global resource market.
    [SerializeField] private ResourceMarketSO rM;
    // Default 
    public float loadSpeed = 1.0f;

    // Set when a location is intiailised
    public List<Resource> resourceMarket;

    // stores resource name and the amount held.
    public List<Tuple<string, float>> Inventory = new List<Tuple<string, float>>();

    public List<string> inDemandResources = new List<string>();
    public List<string> producedResources = new List<string>();
        
    // Facilities
    public SpaceportFacility spaceport;
    public List<ProductionFacility> productionFacilities;

    // Vehicles docked at a location considered as "Active" transactions
    public List<Transaction> dockedVehicles = new List<Transaction>();
    // Vehicles waiting to dock at a location considered as "Inactive" transactions
    public List<Transaction> dockingQueue = new List<Transaction>();
    // Number of vehicles that can dock at a location
    public int dockingCapacity;

    // Booleans that can be used to check
    public bool allowsRefueling;
    public bool allowsConfiguration;
    
    
    public event EventHandler<DockApproveEventArgs> DockApprove;
    public event EventHandler<DockApproveEventArgs> Undock;
    public event EventHandler UpdateResourceDemands;
    
    public override void OnEconomyAwake()
    {
        throw new System.NotImplementedException();
        // Subscribe to transaction handler events
        tH.TransactionApprove += OnTransactionApprove;
        tH.TransactionDeclined += OnTransactionDeclined;

        // Subscribe to Resource Market events if they are needed
    }

    public override void OnEconomyUpdate()
    {
        Debug.Log(locationName + " -> Is updating");
            
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
        // Will need to change some of the logic to account for Undo and Redo functionality.
        foreach (var vehicle in dockedVehicles) {
            // FUCK CARGO MANIFEST VEHICHLE INVENTORY ALL THE WAY
            // foreach (var cargo in vehicle.CargoManifest)
            // {
            //     foreach (var res in inDemandResources.Where(res => cargo.resourceHeld == res))
            //         LoadToInventory(res, cargo.UnloadResource(res, loadSpeed));
            //
            //     foreach (var res in vehicle.ResourcesNeeded.Where(res => producedResources.Contains(res)))
            //         // UnloadFromInventory will check if there is any resource to unload then attempt to unload it.
            //         LoadToInventory(res,cargo.LoadResource(res, UnloadFromInventory(res)));
            // }
        }
    }

    public override void OnEconomyDestroy()
    {
        throw new System.NotImplementedException();
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
        Debug.Log(locationName + " -> Dock request received");
        var newVehicle = new Transaction
        {
            // Information received from vehicle
            InitiatorID = e.VehicleID,
            InitiatingFaction = e.FactionID,
            TargetFaction = factionID,
            //CargoManifest = e.CargoManifest,
            ResourcesNeeded = e.ResourcesOnRoute,
                
            ResourcesTransferred = null,
            //fuelRequired = e.needsFuel,
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
        Debug.Log(locationName +" -> Dock Approve Event Called");
        DockApprove?.Invoke(this, new DockApproveEventArgs {
            Transaction = transaction
        });
    }

    // ensure removal of transaction is done before this is called
    protected virtual void OnUndock(Transaction transaction)
    {
        Undock?.Invoke(this, new DockApproveEventArgs {
            Transaction = transaction
        });
    }
    protected virtual void OnBill(BillEventArgs e)
    {
        
    }

    public virtual void OnTransactionApprove(Transaction t)
    {
        // Possibly create a unique transaction ID as a composite ID e.g an ID string comprised of 
        if (t.TargetID != id) return;
        var match = false;
        foreach (var v in dockedVehicles.Where(v => v.InitiatorID == t.InitiatorID && v.InitiatingFaction == t.InitiatingFaction))
        {
            OnUndock(v);
            // Match still done for error catching.
            match = true;
        }

        if (!match) {
            Debug.LogError("Error in Transaction cannot complete due to error!");
            return;
        }
        
    }

    public virtual void OnTransactionDeclined(Transaction t)
    {
        if (t.TargetID != id) return;
        var match = false;
        foreach (var v in dockedVehicles.Where(v => v.InitiatorID == t.InitiatorID && v.InitiatingFaction == t.InitiatingFaction))
        {
            match = true;
        }

        if (!match) {
            Debug.LogError("Error in Transaction cannot complete due to error!");
            return;
        } 
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
