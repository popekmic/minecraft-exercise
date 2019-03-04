using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Terrain
{
    public class Chunk : MonoBehaviour
    {
        public int size;
        public Vector3Int position;

        private MeshGenerator meshGenerator;
        private Mesh mesh;
        private MeshCollider col;
        private bool meshReady;
        private TerrainGenerator terrainGenerator;
        private readonly Semaphore semaphore = new Semaphore(1, 1);
        private CubeType[,,] chunkTerrain;

        private void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            col = GetComponent<MeshCollider>();
            meshGenerator = new MeshGenerator();
        }

        public void Initialize(TerrainGenerator generator, float delay)
        {
            this.terrainGenerator = generator;
            StartCoroutine(StartMeshGenerating(delay));
        }

        public void StartMeshGenerating()
        {
            new Thread(GenerateMesh).Start();
        }

        private IEnumerator StartMeshGenerating(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartMeshGenerating();
        }

        private void Update()
        {
            if (meshReady)
            {
                ApplyMesh();
                meshReady = false;
            }
        }

        private void GenerateMesh()
        {
            semaphore.WaitOne();
            meshGenerator.Reset();

            Vector3Int chunkLowerBound = new Vector3Int(position.x - 1, position.y - 1, position.z - 1);
            Vector3Int chunkUpperBound = new Vector3Int(position.x + size, position.y + size, position.z + size);
            chunkTerrain = terrainGenerator.GetChunkTerrain(chunkLowerBound, chunkUpperBound);

            for (int x = position.x; x < position.x + size; x++)
            {
                for (int y = position.y; y < position.y + size; y++)
                {
                    for (int z = position.z; z < position.z + size; z++)
                    {
                        meshGenerator.GenerateCubeMesh(GetCubeTypeAtPosition, x, y, z);
                    }
                }
            }

            meshReady = true;
        }

        private CubeType GetCubeTypeAtPosition(int x, int y, int z)
        {
            return chunkTerrain[
                Math.Abs(x % (size + 2)),
                Math.Abs(y % (size + 2)),
                Math.Abs(z % (size + 2))];
        }

        private void ApplyMesh()
        {
            mesh.Clear();
            mesh.vertices = meshGenerator.vertices.ToArray();
            mesh.uv = meshGenerator.uvs.ToArray();
            mesh.triangles = meshGenerator.triangles.ToArray();
            mesh.RecalculateNormals();

            col.sharedMesh = null;
            col.sharedMesh = mesh;

            semaphore.Release();
        }
    }
}