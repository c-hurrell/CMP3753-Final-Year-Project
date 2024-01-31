using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace HarbingerScripts
{
    public class Region : MonoBehaviour
    {
        [Serializable] public struct Production
        {
            // Resource it produces
            public string resource;
            
            // Upgrades with the local development level in response to supply and demand
            public int level;
            public float amountPerTick;
            
            // If max jobs isn't filled have a percentage reduction
            public int maxJobs;
            public int filledJobs;
            
            
            // To be relative to the regions position?
            public Vector3 productionPos;
        }
        
        [Header("Region Information")]
        [SerializeField] public int regionID;
        [SerializeField] public string regionName;
        [SerializeField] public Vector3 regionPosition;
        
        private GameObject _globalEconomy;

        [Header("Local Economy")] 
        [SerializeField] public int population;
        [SerializeField] private float popGrowth;
        [SerializeField] public List<GlobalEconomy.Resource> regionResources;
        [SerializeField] private List<Production> regionProduction;

        // Start is called before the first frame update
        private void Start()
        {
            var global = GameObject.FindGameObjectsWithTag("GlobalEconomy");
            if (global.Length > 1)
            {
                Debug.LogWarning("There can't be two global economy objects in one scene!");
            }

            _globalEconomy = global[0];
            
            var globalResources = _globalEconomy.GetComponent<GlobalEconomy>().resources;
            regionResources = globalResources;
            regionPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
