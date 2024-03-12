using System.Collections.Generic;
using UnityEngine;
using HarbingerCore;

namespace HarbingerScripts
{
    public class RegionManager : MonoBehaviour
    {
        //[SerializeField] public Region region;
        
        
        void Awake()
        {
            // region.regionIdentity.regionCentre = transform.position;
            // foreach (var child in transform.GetComponentsInChildren<LocationManager>())
            // {
            //     child.location.belongsToRegion = region.regionIdentity;
            //     region.locations.Add(child.location.identifier.locationID);
            // }

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

