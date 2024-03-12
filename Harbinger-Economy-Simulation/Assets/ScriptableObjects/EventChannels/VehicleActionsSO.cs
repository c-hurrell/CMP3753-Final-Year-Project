using System.Collections;
using System.Collections.Generic;
using HarbingerCore;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VehicleActions", menuName = "Events/VehicleActions")]
public class VehicleActionsSO : ScriptableObject
{
    public UnityAction<DockEventArgs> RequestDock;
    public UnityAction<int, int, VehicleAction> VehicleRequest;
    
    public void OnRequestDock(DockEventArgs e)
    {
        if(RequestDock != null)
            RequestDock.Invoke(e);
        else
            Debug.LogWarning("RequestDock event not subscribed to!");
    }

    public void OnVehicleRequest(int vehicleID, int factionID, VehicleAction action)
    {
        if(VehicleRequest != null)
            VehicleRequest.Invoke(vehicleID, factionID, action);
        else
            Debug.LogWarning("VehicleRequest event is not subscribed to!");
    }
}

public enum VehicleAction
{
    Refuel,
    Load,
    Unload,
    Reconfigure
}
