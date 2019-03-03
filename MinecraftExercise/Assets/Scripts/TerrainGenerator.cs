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
    public TerrainInformation Information { get; }

    private Vector2Int currentCenter;
    //private CubeType[,,] precalculated = new CubeType[256, 256, 32];

    public TerrainGenerator(TerrainDefinition definition) : this(definition, new TerrainInformation())
    {
        Information.groundLevel = 10;
        Information.maxHeight = 35;
        Information.noiseOffset = (int) (Random.value * 250);
        Information.changes = new Dictionary<Vector3Int, CubeType>();
    }

    public TerrainGenerator(TerrainDefinition definition, TerrainInformation terrainInformation)
    {
        this.definition = definition;
        this.Information = terrainInformation;
    }

    public void AddChange(CubeType type, int x, int y, int z)
    {
        Information.changes[new Vector3Int(x, y, z)] = type;
    }

    public CubeType GetCubeTypeAtPosition(int x, int y, int z)
    {
        if (y < 0) return CubeType.Air;
        
        Vector3Int coords = new Vector3Int(x,y,z);
        if (Information.changes.ContainsKey(coords))
        {
            return Information.changes[coords];
        }
        
        int terrainHeight = GetTerrainHeight(x, z);
        if (y > terrainHeight)
        {
            return CubeType.Air;
        }

        return definition.GetCubeByTerrainHeight(y);
        /*int coordX = x % 256;
        int coordZ = z % 256;
        return precalculated[coordX < 0 ? coordX + 256 : coordX,coordZ < 0 ? coordZ + 256 : coordZ, y % 32];*/
    }

    private int GetTerrainHeight(int x, int y)
    {
        float scaledX = (Information.noiseOffset + x) / Scale;
        float scaledY = (Information.noiseOffset + y) / Scale;
        int height = Math.Max(
            (int) (Mathf.PerlinNoise(scaledX, scaledY) *
                   Information.maxHeight),
            Information.groundLevel);
        return height;
    }
}