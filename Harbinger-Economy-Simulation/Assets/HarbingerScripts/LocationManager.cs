using System;
using System.Data;
using HarbingerCore;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace HarbingerScripts
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] public Location location;
        
        [SerializeField] public GameObject economyManager;

        [SerializeField] private VoidEventChannelSO EconomyAwake;
        [SerializeField] private VoidEventChannelSO EconomyUpdate;

        private void OnEnable()
        {
            EconomyAwake.OnEventRaised += location.OnEconomyAwake;
            EconomyUpdate.OnEventRaised += location.OnEconomyUpdate;
        }

        private void OnDisable()
        {
            EconomyAwake.OnEventRaised -= location.OnEconomyAwake;
            EconomyUpdate.OnEventRaised -= location.OnEconomyUpdate;
        }
        
        
        // When location is loading it needs to fetch the resources available in the global market
        private void Awake()
        {
            // If it hasn't been set manually fetch global economy
            if (economyManager == null) {
                economyManager = GameObject.FindGameObjectWithTag("GlobalEconomy");
            }
            
            // Sets location and global resource market
            location.position = transform.position;
            foreach(var resource in economyManager.GetComponent<EconomyManager>().globalResourceMarket) 
                location.resourceMarket.Add(new Resource
                {
                    resourceInfo = resource,
                    currentValue = resource.baseVal,
                    demand = 0
                });
        }
        private void OnTriggerEnter(Collider other)
        {
            // If object is a vehicle listen for the the specific dock request
            if (other.CompareTag("Vehicle"))
            {
                other.GetComponent<VehicleController>().vehicleActions.RequestDock += location.DockRequest;
                other.GetComponent<VehicleController>().vehicleActions.VehicleRequest += location.OnVehicleRequest;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // If object is a vehicle stop listening for that specific dock request
            if (other.CompareTag("Vehicle"))
            {
                other.GetComponent<VehicleController>().vehicleActions.RequestDock -= location.DockRequest;
                other.GetComponent<VehicleController>().vehicleActions.VehicleRequest += location.OnVehicleRequest;
            }
        }
    }
}
