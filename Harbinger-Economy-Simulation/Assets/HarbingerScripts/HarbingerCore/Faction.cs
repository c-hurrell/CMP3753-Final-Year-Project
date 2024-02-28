using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
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

        public static event EventHandler<ApprovalEventArgs> Approval;
        
        public virtual void InitFaction()
        {
            Location.Bill += OnBill;
        }
        public virtual void OnBill(object source, BillEventArgs e)
        {
            // Ignore if not for this faction
            if (e.FactionID != factionIdentity.id) return;
            
            // If its money received don't need to check if I have the money
            if (e.Currency >= 0) {
                credits += e.Currency;
                return;
            }
            
            // Check if transaction will push balance below 0 (may change to a tweak-able threshold later)
            if ((credits + e.Currency) <= 0)
            {
                OnApproval(new ApprovalEventArgs
                {
                    LocationID = e.LocationID,
                    Status = false,
                    FactionID = e.FactionID,
                    VehicleID = e.VehicleID
                });
                return;
            }
            
            // If I have enough money
            credits += e.Currency;
            OnApproval( new ApprovalEventArgs
            {
                LocationID = e.LocationID,
                Status = true,
                FactionID = e.FactionID,
                VehicleID = e.VehicleID
            });
        }

        protected static void OnApproval(ApprovalEventArgs e)
        {
            Approval?.Invoke(null, e);
        }

        public void OnDestroy()
        {
            // unsubscribe from all events
            Location.Bill -= OnBill;
        }
    }

    public class ApprovalEventArgs : EventArgs
    {
        public int LocationID { get; set; }
        public bool Status { get; set; }
        
        // Transaction Identfiers -> What about transactions outside of vehicle based transactions?
        public int FactionID { get; set; }
        public int VehicleID { get; set; }
    }
}
