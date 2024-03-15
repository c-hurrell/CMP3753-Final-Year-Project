using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TransactionHandler", menuName = "Events/TransactionHandler")]
public class TransactionHandlerSO : ScriptableObject
{
    public UnityAction<Transaction> Bill;
    public UnityAction<Transaction> Receive;
    // Implement other transaction types
    // Locations will listen for these events as well as UI to see completed transactions
    public UnityAction<Transaction> TransactionApprove;
    public UnityAction<Transaction> TransactionDeclined;

    public void OnBill(Transaction transaction)
    {
        if (Bill != null)
            Bill.Invoke(transaction);
        else 
            Debug.LogWarning("Bill Event is not subscribed to!");
    }

    public void OnReceive(Transaction transaction)
    {
        if (Receive != null)
            Bill.Invoke(transaction);
        else
            Debug.LogWarning("Receive Event is not subscribed to!");
    }
    
    public void OnTransactionApprove(Transaction transaction)
    {
        if (TransactionApprove != null)
            TransactionApprove.Invoke(transaction);
        else 
            Debug.LogWarning("TransactionApprove Event is not subscribed to!");
    }

    public void OnTransactionDeclined(Transaction transaction)
    {
        if (TransactionDeclined != null)
            TransactionDeclined.Invoke(transaction);
        else 
            Debug.LogWarning("TransactionApprove Event is not subscribed to!");
    }
    
}
