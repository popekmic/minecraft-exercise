using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainSerialization
{
    public int noiseOffset;
    public int maxHeight;
    public int groundLevel;
    public Dictionary<SerializableVector3Int, CubeType> changes;

    public TerrainSerialization(TerrainInformation info)
    {
        noiseOffset = info.noiseOffset;
        maxHeight = info.maxHeight;
        groundLevel = info.groundLevel;
        
        changes = new Dictionary<SerializableVector3Int, CubeType>();
        
        foreach (var key in info.changes.Keys)
        {
            changes[new SerializableVector3Int(key)] = info.changes[key];
        }
    }

    public TerrainInformation ToTerrainInfo()
    {
        TerrainInformation result = new TerrainInformation();
        result.noiseOffset = noiseOffset;
        result.maxHeight = maxHeight;
        result.groundLevel = groundLevel;
        result.changes = new Dictionary<Vector3Int, CubeType>();
        foreach (var key in changes.Keys)
        {
            result.changes[key.ToVector3Int()] = changes[key];
        }

        return result;
    }

    [Serializable]
    public class SerializableVector3Int
    {
        private readonly int x;
        private readonly int y;
        private readonly int z;

        public SerializableVector3Int(Vector3Int vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public Vector3Int ToVector3Int()
        {
            return new Vector3Int(x, y, z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(GetType() == obj.GetType()))
            {
                return false;
            }

            SerializableVector3Int other = (SerializableVector3Int) obj;
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                hash = hash * 23 + z.GetHashCode();
                return hash;
            }
        }
    }
}