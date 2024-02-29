using System;
using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] public List<ResourceIdentifier> globalResourceMarket;

        [SerializeField] public List<FactionIdentifier> factions;
        private GameObject[] _factionObjects;
        
        [SerializeField] public List<RegionIdentifier> regions;
        private GameObject[] _regionObjects;

        [SerializeField] public List<LocationIdentifier> locations;
        private GameObject[] _locationObjects;

        [SerializeField] public List<Vehicle> availableVehicles;
        
        [SerializeField] private float timestep = 0.02f;
        private float _counter = 0;

        private void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            _regionObjects = GameObject.FindGameObjectsWithTag("Region");
            // Initializes each region with the resources.
            foreach (var regionObject in _regionObjects)
            {
                // Will add automatic assignment with sorted list later it drove me crazy enough last time for now it can be manual.
                regions.Add(regionObject.GetComponent<RegionManager>().region.regionIdentity);
            }
            
            _locationObjects = GameObject.FindGameObjectsWithTag("Location");
            foreach (var locationObject in _locationObjects)
            {
                locations.Add(locationObject.GetComponent<LocationManager>().location.identifier);
            }
            
            _factionObjects = GameObject.FindGameObjectsWithTag("Faction");

            foreach (var factionObject in _factionObjects)
            {
                factions.Add(factionObject.GetComponent<FactionManager>().faction.factionIdentity);
            }


        }
        private void FixedUpdate()
        {
            // Adjust for time step if needed
            if (!(_counter >= 1 / timestep)) {
                _counter += 1;
                return;
            }
            foreach (var location in _locationObjects)
            {
                location.GetComponent<LocationManager>().location.EconomyUpdate();
            }
        }
    }
}
