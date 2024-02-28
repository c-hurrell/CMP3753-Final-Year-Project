using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable] public class Facility
    {
        public string name;
        public int level;
        public int levelCap = 5;
        public List<Tuple<string, float>> ResourceDemand;
        // Construction Requirements

        public virtual void UpdateLevel(int levelChange)
        {
            level += levelChange;
            // implement specific changes here...
        }
    }
    [Serializable] public class SpaceportFacility : Facility
    {
        public int capacity;
        public float loadingSpeed;

        public override void UpdateLevel(int levelChange)
        {
            base.UpdateLevel(levelChange);
            switch (level)
            {
                case 0:
                    capacity = 1;
                    loadingSpeed = 5.0f;
                    break;
                case 1:
                    capacity = 4;
                    loadingSpeed = 7.5f;
                    break;
                case 2:
                    capacity = 8;
                    loadingSpeed = 10.0f;
                    break;
                case 3:
                    capacity = 10;
                    loadingSpeed = 15.0f;
                    break;
                case 4:
                    capacity = 12;
                    loadingSpeed = 20.0f;
                    break;
                case 5:
                    capacity = 15;
                    loadingSpeed = 20.0f;
                    break;
            }
        }
    }
    [Serializable] public class ProductionFacility : Facility
    {
        public List<Tuple<string, float>> ResourceProduction;
    }
}
