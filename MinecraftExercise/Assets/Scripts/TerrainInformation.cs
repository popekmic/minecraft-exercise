using System;
using System.Collections.Generic;
using UnityEngine;


public class TerrainInformation
{
    public int noiseOffset;
    public int maxHeight;
    public int groundLevel;
    public Dictionary<Vector3Int, CubeType> changes;
}