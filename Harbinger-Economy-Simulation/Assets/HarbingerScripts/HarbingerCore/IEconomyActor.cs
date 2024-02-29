using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for objects with similar. Example a faction or location are both economy actors but a vehicle isn't.
public interface IEconomyActor
{
    // will run when an actor is initialised
    public abstract void EconomyAwake();
    
    // will run when called from fixed update all economy actions such as loading unloading are controlled through this
    public abstract void EconomyUpdate();
    
    public abstract void EconomyDestroy();
}
