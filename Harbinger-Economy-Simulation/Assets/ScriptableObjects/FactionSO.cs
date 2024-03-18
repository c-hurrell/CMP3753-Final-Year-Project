using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using HarbingerCore;

[CreateAssetMenu(fileName = "FactionSO", menuName = "Harbinger/Faction")]
public class FactionSO : EconomyActorSO
{
     // Default values are unassigned and ID 0, if a location is unowned it should engage in any logic
     public string factionName = "Unowned";
     public int factionID = 0;
     
     // Other defaults include :
     // - "Neutral" with an ID of 1 this is for a passive faction that has no vehicles.
     // - "Sol Trading Consortium" with an ID of 2 this faction is primarily used for testing vehicle and route logic

     public TransactionHandlerSO tH;
     
     public float credits = 1000000000f;
     
     // How to approach location ownership?
     public List<int> locationsOwned;
     public List<VehicleSO> vehiclesOwned;

     // Subscribe to all events
     public override void OnEconomyAwake()
     {
          tH.Bill += OnBill;
          tH.Receive += OnReceive;
     }

     public override void OnEconomyUpdate()
     {
          // Implement later
          Debug.Log(factionName + " -> Is Updating!");
     }

     // Clean up of event handling
     public override void OnEconomyDestroy()
     {
          tH.Bill -= OnBill;
          tH.Receive -= OnReceive;
     }

     public void OnBill(Transaction t)
     {
          // If the initiating faction owes money charge the initiating faction
          if (t.InitiatingFaction == factionID && t.Total >= 0) {
               if (credits - t.Total < 0)
                    tH.TransactionDeclined(t);
               else {
                    credits -= t.Total;
                    tH.TransactionApprove(t);
               }
          }
          // If the target faction owes money charge the target faction
          else if (t.TargetFaction == factionID && t.Total < 0) {
               if (credits + t.Total < 0) 
                    tH.TransactionDeclined(t);
               else {
                    credits += t.Total;
                    tH.TransactionApprove(t);
               }
          }
     }

     public void OnReceive(Transaction t)
     {
          if (t.InitiatingFaction == factionID && t.Total < 0)
               credits -= t.Total;
          
          else if (t.TargetFaction == factionID && t.Total >= 0)
               credits += t.Total;
     }
     
     
}
