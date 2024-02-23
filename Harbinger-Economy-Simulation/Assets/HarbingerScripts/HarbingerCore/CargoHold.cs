using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class CargoHold
    {
        public float capacity;
      
        public ResourceType holdsType;
        
        public string resourceHeld;
        public float amountHeld;

        public bool isLoading;
        
        // Returns the amount that is loaded into the hold. This meaning the amount should be the amount attempted to load.
        public float LoadResource(string resourceName, float amount)
        {
            // If a different resource is held don't load resource
            if (resourceName != resourceHeld && amountHeld > 0) {
                isLoading = false;
                return 0;
            }

            isLoading = true;
            
            resourceHeld = resourceName;
            
            if (amountHeld + amount >= capacity) {
                var loaded =  amount - ((amountHeld + amount) - capacity);
                amountHeld += loaded;
                isLoading = false;
                return loaded;
            }
            
            amountHeld += amount;
            return amount;
        }

        // Returns the amount that is unloaded out of the hold. This is the amount should be attempted to unloaded each update
        public float UnloadResource(string resourceName, float amount) {
            // If you are attempting to unload a resource that isn't on the ship don't unload
            if (resourceName != resourceHeld || amountHeld <= 0) {
                isLoading = false;
                return 0;
            }

            isLoading = true;

            if (amountHeld - amount <= 0) {
                var unloaded = amount - (amountHeld - amount);
                amountHeld -= unloaded;
                isLoading = false;
                return unloaded;
            }

            amountHeld -= amount;
            
            return amount;
        }
    }
}
