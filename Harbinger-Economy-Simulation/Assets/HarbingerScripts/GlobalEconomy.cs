using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HarbingerScripts
{
    public class GlobalEconomy : MonoBehaviour
    {
        [Serializable] public struct Resource
         { 
             public string name;
             public float baseValue; 
             public float currentValue;
             public float stored; 
             public float demand;
         }
        [Serializable] public struct RegionStorage
        {
            public int regionID;
            public string regionName;
        }
        
        public List<Resource> resources;

        [SerializeField] private List<string> regionNames;
        
        private List<Region> _regions = new List<Region>();

        public List<RegionStorage> regions;
        // Start is called before the first frame update
        private void Start()
        {
            // Finds all regions and assigns them and ID
            var regionList = GameObject.FindGameObjectsWithTag("Region");
            for (var i = 0; i < regionList.Length; i++)
            {
                _regions.Add(regionList[i].GetComponent<Region>());
                _regions[i].regionID = i;
                if (_regions[i].regionName == "")
                {
                    var ran = Random.Range(0, regionNames.Count);
                    _regions[i].regionName = regionNames[ran];
                    regionNames.RemoveAt(ran);
                }
                AddRegion(i, _regions[i].regionName);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void AddRegion(int i, string regName)
        {
            var regionStorage = new RegionStorage
            {
                regionID = i,
                regionName = regName
            };
            regions.Add(regionStorage);
        }
    }
}
