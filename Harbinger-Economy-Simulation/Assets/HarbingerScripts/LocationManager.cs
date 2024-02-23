using System.Data;
using HarbingerCore;
using UnityEngine;
using UnityEngine.Video;

namespace HarbingerScripts
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] public Location location;
        
        [SerializeField] public GameObject economyManager;

        
        // When location is loading it needs to fetch the resources available in the global market
        private void Awake()
        {
            location.position = transform.position;
            foreach(var resource in economyManager.GetComponent<EconomyManager>().globalResourceMarket) 
                location.resourceMarket.Add(new Resource
                {
                    resourceInfo = resource,
                    currentValue = resource.baseVal,
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
