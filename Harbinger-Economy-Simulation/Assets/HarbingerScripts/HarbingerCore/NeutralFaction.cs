using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarbingerCore
{
    public class NeutralFaction : Faction
    {
        public override void InitFaction()
        {

        }

        public override void OnTransaction(object source, TransactionEventArgs e)
        {
            //base.OnTransaction(source, e); 
        }

    }
}
