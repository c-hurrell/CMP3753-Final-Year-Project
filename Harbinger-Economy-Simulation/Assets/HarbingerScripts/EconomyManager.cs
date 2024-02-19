using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] public List<ResourceIdentifier> globalResourceMarket;

        [SerializeField] public List<FactionIdentifier> factions;
        [SerializeField] private GameObject[] factionObjects;
        
        [SerializeField] public List<RegionIdentifier> regions;
        [SerializeField] private GameObject[] regionObjects;

        [SerializeField] public List<Vehicle> availableVehicles;

        private void Awake()
        {
            
        }

        private void FixedUpdate()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            regionObjects = GameObject.FindGameObjectsWithTag("Region");
            // Initializes each region with the resources.
            foreach (var regionObject in regionObjects)
            {
                // Will add automatic assignment with sorted list later it drove me crazy enough last time for now it can be manual.
                regions.Add(regionObject.GetComponent<RegionManager>().region.regionIdentity);
            }

            factionObjects = GameObject.FindGameObjectsWithTag("Faction");

            foreach (var factionObject in factionObjects)
            {
                factions.Add(factionObject.GetComponent<FactionManager>().faction.factionIdentity);
            }


        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void EcoTransaction(int payingFaction, int receivingFaction, float amount)
        {
            
        }
    }
}
