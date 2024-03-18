using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EconomyActorSO : ScriptableObject
{
    public VoidEventChannelSO eAwake;
    public VoidEventChannelSO eUpdate;
    public VoidEventChannelSO eDestroy;

    public abstract void OnEconomyAwake();
    public abstract void OnEconomyUpdate();
    public abstract void OnEconomyDestroy();
}
