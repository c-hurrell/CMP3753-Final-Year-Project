using System;
using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] public List<ResourceIdentifier> globalResourceMarket;

        [SerializeField] VoidEventChannelSO EconomyAwake;

        [SerializeField] private VoidEventChannelSO EconomyUpdate;
        
        //public static event EventHandler EconomyAwake;

        //public static event EventHandler EconomyUpdate;

        //public static event EventHandler EconomyDestroy;

        [SerializeField] public List<FactionIdentifier> factions;

        [SerializeField] public List<RegionIdentifier> regions;

        [SerializeField] public List<LocationIdentifier> locations;

        [SerializeField] public List<Vehicle> availableVehicles;

        [SerializeField] private float timestep = 0.02f;
        private float _counter = 0;
        [SerializeField] public float economySpeed = 1;

        // if there is an object matching this destroy this
        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            // Done here so everything is initialised correctly
            EconomyAwake.RaiseEvent();
        }

        private void FixedUpdate()
        {
            // Adjust for time step if needed - kinda jank tho
            if (!(_counter >= 1 / timestep))
            {
                _counter += 1 * economySpeed;
                return;
            }

            _counter = 0;
            Debug.Log(" : Updating Economy!");
            EconomyUpdate.RaiseEvent();
        }

        public void SetSpeed(float speed)
        {
            // changing to be accessed by 
            economySpeed = speed;
        }
    }
}
