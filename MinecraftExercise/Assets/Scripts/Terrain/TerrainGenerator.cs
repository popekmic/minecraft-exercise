using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class TerrainGenerator
    {
        private const float Scale = 40;

        private TerrainDefinition definition;
        public TerrainInformation Information { get; }

        private Vector2Int currentCenter;

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
            return GetCubeTypeAtPosition(x, y, z, GetTerrainHeight(x, z));
        }

        private CubeType GetCubeTypeAtPosition(int x, int y, int z, int terrainHeight)
        {
            if (y < 0)
            {
                return CubeType.Air;
            }

            Vector3Int coords = new Vector3Int(x, y, z);
            if (Information.changes.ContainsKey(coords))
            {
                return Information.changes[coords];
            }

            if (y > terrainHeight)
            {
                return CubeType.Air;
            }

            return definition.GetCubeByTerrainHeight(y);
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

        public CubeType[,,] GetChunkTerrain(Vector3Int lowerBound, Vector3Int upperBound)
        {
            int chunkSize = Math.Abs(lowerBound.x - upperBound.x) + 1;
            CubeType[,,] result = new CubeType[chunkSize, chunkSize, chunkSize];

            for (int x = lowerBound.x; x <= upperBound.x; x++)
            {
                for (int z = lowerBound.z; z <= upperBound.z; z++)
                {
                    int terrainHeight = GetTerrainHeight(x, z);
                    for (int y = lowerBound.y; y <= upperBound.y; y++)
                    {
                        result[
                            Math.Abs(x % chunkSize),
                            Math.Abs(y % chunkSize),
                            Math.Abs(z % chunkSize)] = GetCubeTypeAtPosition(x, y, z, terrainHeight);
                    }
                }
            }

            return result;
        }
    }
}