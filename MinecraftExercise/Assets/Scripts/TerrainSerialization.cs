using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainSerialization
{
    public int noiseOffset;
    public int maxHeight;
    public int groundLevel;
    public Dictionary<Vector3Int, CubeType> changes;
}