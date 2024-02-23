using System;
using HarbingerCore;
using UnityEngine;

namespace HarbingerCore
{
    public enum ResourceType
    {
        Solid,
        Liquid,
        Gas,
        Passenger
    }
    [Serializable] public struct ResourceIdentifier
    {
        public string name;
        public int id;
        public ResourceType type;
        public float baseVal;
    }
    [Serializable] public class Resource
    {
        // Base resource information
        public ResourceIdentifier resourceInfo;
        // The current value due to supply/demand
        public float currentValue;
        // Amount stored locally
        public float stored; 
        // Amount needed locally
        public float demand;
    }
}