using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TransactionHandler", menuName = "Events/TransactionHandler")]
public class TransactionHandlerSO : ScriptableObject
{
    public UnityAction<Transaction> Transaction;
    public UnityAction<StationTransaction> StationTransaction;
    // Implement other transaction types

    public void OnTransaction(Transaction transaction)
    {
        if (Transaction != null)
            Transaction.Invoke(transaction);
        else 
            Debug.LogWarning("Transaction Event is not subscribed to!");
    }

    public void OnStationTransaction(StationTransaction transaction)
    {
        if (StationTransaction != null)
            StationTransaction.Invoke(transaction);
        else 
            Debug.LogWarning("StationTransaction Event is not subscribed to!");
    }
}
