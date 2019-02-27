using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class CubeFactory
{
    private readonly int StartingCapacity;
    
    private Dictionary<CubeType, ObjectPool> pools = new Dictionary<CubeType, ObjectPool>();


    public CubeFactory(int startingCapacity)
    {
        StartingCapacity = startingCapacity;
    }

    public void AddOption(CubeType type, GameObject prefab)
    {
        pools[type] = new ObjectPool(prefab, StartingCapacity);
    }

    public GameObject GetCube(CubeType type)
    {
        return pools.ContainsKey(type) ? pools[type].GetObject() : null;
    }
}