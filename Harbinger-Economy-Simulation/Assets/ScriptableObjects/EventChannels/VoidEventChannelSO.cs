using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// This event channel SO is made to hold all variants of void events. This bypasses the need for static events within the project
[CreateAssetMenu(fileName = "Void Event", menuName = "Events/Void Event")]
public class VoidEventChannelSO : ScriptableObject
{
    public string eventName = "Not Assigned!";
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        if(OnEventRaised != null)
            OnEventRaised.Invoke();
        else
            Debug.LogWarning("Event is not subscribed to!");
    }
}
