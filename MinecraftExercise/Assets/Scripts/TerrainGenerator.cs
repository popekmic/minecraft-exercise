using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;
using Utilities;
using Random = UnityEngine.Random;

public class TerrainGenerator
{
    private const float Scale = 40;

    private TerrainDefinition definition;
    private readonly TerrainSerialization terrainSerialization;

    private Vector2Int currentCenter;
    private CubeType[,,] precalculated = new CubeType[256, 256, 32];

    public TerrainGenerator(TerrainDefinition definition) : this(definition, new TerrainSerialization())
    {
        terrainSerialization.groundLevel = 4;
        terrainSerialization.maxHeight = 25;
        terrainSerialization.noiseOffset = (int) (Random.value * 250);
        terrainSerialization.changes = new Dictionary<Vector3Int, CubeType>();
        PrecalculateCubes();
    }

    public TerrainGenerator(TerrainDefinition definition, TerrainSerialization terrainSerialization)
    {
        this.definition = definition;
        this.terrainSerialization = terrainSerialization;
    }

    public void AddChange(CubeType type, int x, int y, int z)
    {
        terrainSerialization.changes[new Vector3Int(x, y, z)] = type;
    }

    private void PrecalculateCubes()
    {
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                int terrainHeight = GetTerrainHeight(x, y);

                for (int z = 0; z < 32; z++)
                {
                    if (z > terrainHeight)
                    {
                        precalculated[x, y, z] = CubeType.Air;
                        continue;
                    }

                    precalculated[x, y, z] = definition.GetCubeByTerrainHeight(z);
                }
            }
        }
    }

    public CubeType GetCubeTypeAtPosition(int x, int y, int z)
    {
        if (y < 0) return CubeType.Air;
        
        Vector3Int coords = new Vector3Int(x,y,z);
        if (terrainSerialization.changes.ContainsKey(coords))
        {
            return terrainSerialization.changes[coords];
        }

        int coordX = x % 256;
        int coordZ = z % 256;
        return precalculated[coordX < 0 ? coordX + 256 : coordX,coordZ < 0 ? coordZ + 256 : coordZ, y % 32];
    }

    private int GetTerrainHeight(int x, int y)
    {
        float scaledX = (terrainSerialization.noiseOffset + x) / Scale;
        float scaledY = (terrainSerialization.noiseOffset + y) / Scale;
        int height = Math.Max(
            (int) (Mathf.PerlinNoise(scaledX, scaledY) *
                   terrainSerialization.maxHeight),
            terrainSerialization.groundLevel);
        return height;
    }
}