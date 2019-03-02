using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class TerrainDefinition
{
   
    private SortedDictionary<int,CubeType> heightTypes = new SortedDictionary<int,CubeType>();

    public void AddOption(CubeType type,int height)
    {
        heightTypes[height] = type;
    }

    public CubeType GetCubeByTerrainHeight(int height)
    {
        CubeType resultType = CubeType.Air;
        
        foreach (var key in heightTypes.Keys)
        {
            if (height >= key)
            {
                resultType = heightTypes[key];
            }
        }
        
        return resultType;
    }
}