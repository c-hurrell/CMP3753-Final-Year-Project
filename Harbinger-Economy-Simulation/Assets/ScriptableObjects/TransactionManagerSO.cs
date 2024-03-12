using System.Collections;
using System.Collections.Generic;
using HarbingerCore;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TransactionManager", menuName = "ScriptableObjects/TM")]
public class TransactionManagerSO : ScriptableObject
{
    public UnityEvent<Transaction> transactionEvent;
}
