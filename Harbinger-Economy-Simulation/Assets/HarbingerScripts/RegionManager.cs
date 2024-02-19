using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class RegionManager : MonoBehaviour
    {
        [SerializeField] public Region region;
        
        [SerializeField] public GameObject economyManager;
        void Awake()
        {
            region.regionIdentity.regionCentre = transform.position;
            foreach (var child in transform.GetComponentsInChildren<LocationManager>())
            {
                child.location.belongsToRegion = region.regionIdentity;
                region.locations.Add(child.location.locationID);
            }
            foreach(var resource in economyManager.GetComponent<EconomyManager>().globalResourceMarket) 
                region.resourceMarket.Add(new Resource
                {
                    resourceInfo = resource,
                    currentValue = resource.baseVal,
                    stored = 0,
                    demand = 0
                });
        }
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

