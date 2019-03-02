using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Utilities;

namespace DefaultNamespace
{
    public class TerrainHandler
    {
        public const int ChunkSize = 16;
        public const int SpaceHeight = 2;

        private readonly TerrainGenerator terrainGenerator;
        private readonly int viewDistance;
        private readonly ObjectPool chunkPool;
        private Vector2Int currentCenter;

        private readonly Dictionary<Vector2Int, List<Chunk>> chunks =
            new Dictionary<Vector2Int, List<Chunk>>();

        public TerrainHandler(TerrainGenerator terrainGenerator, GameObject chunkPrefab, int viewDistance)
        {
            this.terrainGenerator = terrainGenerator;
            this.viewDistance = viewDistance / ChunkSize;
            currentCenter = new Vector2Int(this.viewDistance / 2, this.viewDistance / 2);
            chunkPool = new ObjectPool(chunkPrefab, (int) Mathf.Pow(this.viewDistance * 2, 2));
            GenerateTerrain();
        }

        public void AddChange(CubeType type, int x, int y, int z)
        {
            Debug.Log("Change:" + x + " " + y + " " + z);
            terrainGenerator.AddChange(type, x, y, z);
            foreach (var chunk in chunks[new Vector2Int(x/ChunkSize,z/ChunkSize)])
            {
                if (chunk.position.y / ChunkSize == y / ChunkSize)
                {
                    Debug.Log("Started generating at mesh : " + chunk.position);
                    chunk.StartMeshGenerating();
                    break;
                }
            }
        }

        private void GenerateTerrain()
        {
            for (int x = currentCenter.x - viewDistance; x < currentCenter.x + viewDistance; x++)
            {
                for (int y = currentCenter.y - viewDistance; y < currentCenter.y + viewDistance; y++)
                {
                    for (int z = 0; z < SpaceHeight; z++)
                    {
                        CreateChunkAtPosition(x, y, z, 0);
                    }
                }
            }
        }

        private void CreateChunkAtPosition(int x, int y, int height, float delay)
        {
            GameObject chunkObject = chunkPool.GetObject();
            Chunk chunk = chunkObject.GetComponent<Chunk>();
            chunk.size = ChunkSize;
            chunk.position = new Vector3Int(x * ChunkSize, height * ChunkSize, y * ChunkSize);
            chunk.Initialize(terrainGenerator, delay);
            Vector2Int coords = new Vector2Int(x, y);
            if (!chunks.ContainsKey(coords))
            {
                chunks[coords] = new List<Chunk>();
            }

            chunks[coords].Add(chunk);
        }

        public void Move(Vector2Int change)
        {
            change = new Vector2Int(change.x / ChunkSize, change.y / ChunkSize);
            Vector2Int newCenter = currentCenter + change;

            int searchDistanceX = viewDistance + Math.Abs(change.x);
            int searchDistanceY = viewDistance + Math.Abs(change.y);
            int newGeometryCount = 0;

            for (int x = currentCenter.x - searchDistanceX; x < currentCenter.x + searchDistanceX; x++)
            {
                for (int y = currentCenter.y - searchDistanceY; y < currentCenter.y + searchDistanceY; y++)
                {
                    var coordinates = new Vector2Int(x, y);
                    if (x >= newCenter.x - viewDistance
                        && x < newCenter.x + viewDistance
                        && y >= newCenter.y - viewDistance
                        && y < newCenter.y + viewDistance)
                    {
                        if (!chunks.ContainsKey(coordinates))
                        {
                            for (int z = 0; z < SpaceHeight; z++)
                            {
                                CreateChunkAtPosition(x, y, z, 0.075f * newGeometryCount++);
                            }
                        }


                        continue;
                    }

                    ReturnChunk(coordinates);
                }
            }

            currentCenter = newCenter;
        }

        private void ReturnChunk(Vector2Int coordinates)
        {
            if (chunks.ContainsKey(coordinates))
            {
                foreach (var chunk in chunks[coordinates])
                {
                    chunkPool.ReturnObject(chunk.gameObject);
                }

                chunks.Remove(coordinates);
            }
        }
    }
}