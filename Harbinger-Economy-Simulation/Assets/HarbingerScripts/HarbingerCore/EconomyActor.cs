using System;
using System.Collections;
using System.Collections.Generic;
using HarbingerScripts;
using UnityEngine;

namespace HarbingerCore
{
    [Serializable]
    public abstract class EconomyActor
    {
        public abstract void OnEconomyAwake();

        public abstract void OnEconomyUpdate();

        public abstract void OnEconomyDestroy();

    }
}
