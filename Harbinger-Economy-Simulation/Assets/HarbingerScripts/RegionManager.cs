using System.Collections.Generic;
using UnityEngine;

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

            region.resourceMarket = economyManager.GetComponent<EconomyManager>().globalResourceMarket;
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

