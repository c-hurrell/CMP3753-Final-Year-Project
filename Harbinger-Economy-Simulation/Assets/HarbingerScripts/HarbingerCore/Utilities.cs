using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    // This is done so I have a viewable input in the editor may change as UI is added in
    [Serializable] public class ResAmountPair
    {
        public string resource;
        public float amount;
    }
}
