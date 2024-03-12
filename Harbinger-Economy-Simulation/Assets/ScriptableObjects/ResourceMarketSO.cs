using System.Collections;
using System.Collections.Generic;
using HarbingerCore;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceMarketSO", menuName = "Harbinger/ResourceMarket")]
public class ResourceMarketSO : ScriptableObject
{
    public List<ResourceIdentifier> resources;
}
