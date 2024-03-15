using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using HarbingerCore;

[CreateAssetMenu(fileName = "FactionSO", menuName = "Harbinger/Faction")]
public class FactionSO : ScriptableObject
{
     public string factionName = "Unsigned";
     public int factionID = 0;

     public TransactionHandlerSO tH;
     
     public float credits = 1000000000f;
     
     public List<int> locationsOwned;
     public List<Vehicle> vehiclesOwned;

     // Subscribe to all events
     public void OnEconomyAwake()
     {
          tH.Bill += OnBill;
          tH.Receive += OnReceive;
     }

     public void OnEconomyUpdate()
     {
          // Implement later
          Debug.Log(factionName + " -> Is Updating!");
     }

     // Clean up of event handling
     public void OnEconomyDestroy()
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

     public void OnLocationTransaction(StationTransaction t)
     {
          
     }
     
     
}
