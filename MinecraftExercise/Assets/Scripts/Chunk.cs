using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Profiling;

public class Chunk : MonoBehaviour
{
    public int size;
    public Vector3Int position;

    private MeshGenerator meshGenerator;
    private Mesh mesh;
    private MeshCollider col;
    private bool shouldRecreateMesh;
    private bool meshReady;

    private readonly Semaphore semaphore = new Semaphore(1,1);
    
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();
    }

    public void Initialize(TerrainGenerator generator, float delay)
    {
        StartCoroutine(StartMeshGenerating(generator, delay));
    }

    public void StartMeshGenerating()
    {
        shouldRecreateMesh = true;
    }

    private IEnumerator StartMeshGenerating(TerrainGenerator generator, float delay)
    {
        yield return new WaitForSeconds(delay);
        meshGenerator = new MeshGenerator(generator);
        shouldRecreateMesh = true;
    }

    private void Update()
    {
        if (shouldRecreateMesh)
        {
            new Thread(GenerateMesh).Start();
            shouldRecreateMesh = false;
        }

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

        for (int x = position.x; x < position.x + size; x++)
        {
            for (int y = position.y; y < position.y + size; y++)
            {
                for (int z = position.z; z < position.z + size; z++)
                {
                    meshGenerator.GenerateCubeMesh(x, y, z);
                }
            }
        }

        meshReady = true;
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