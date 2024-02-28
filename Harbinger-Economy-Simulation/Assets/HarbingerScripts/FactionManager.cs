using System;
using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class FactionManager : MonoBehaviour
    {
        [SerializeField] public GameObject economyManager;
        [SerializeField] public Faction faction;
        

        private void Awake()
        {
            faction.InitFaction();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            var locationObjects = GameObject.FindGameObjectsWithTag("Location");
            // On start check if the faction own any locations. This means whatever is bought/sold at this location is billed to them
            // Neutral faction represents a non-expanding faction that acts as a trade point.
            foreach (var locationObject in locationObjects)
            {
                if (locationObject.GetComponent<LocationManager>().location.factionID == faction.factionIdentity.id)
                {
                    faction.locationsOwned.Add(locationObject.GetComponent<LocationManager>().location.identifier.locationID);
                }
            }
            
            if (faction.factionIdentity.id <= 0) return;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
