using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    public class NeutralFaction : Faction
    {
        public override void InitFaction()
        {
            Location.Bill += OnBill;
        }

        public override void OnBill(object source, BillEventArgs e)
        {
            if (e.FactionID != factionIdentity.id) return;
            OnApproval(new ApprovalEventArgs
            {
                LocationID = e.LocationID,
                Status = true,
                FactionID = e.FactionID,
                VehicleID = e.VehicleID
            });
            //base.OnTransaction(source, e); 
        }

    }
}
